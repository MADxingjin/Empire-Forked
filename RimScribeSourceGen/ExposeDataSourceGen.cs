using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RimScribeSourceGen
{
    /// <summary>
    /// First,find all class with fields marked.
    /// </summary>
    internal class SyntaxRecv : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassDeclarations { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode Node)
        {
            Console.WriteLine($"[ExposeDataGenerator] Inspecting Syntax Tree Node {Node.Kind().ToString()}");
            if (Node is ClassDeclarationSyntax cd1 &&
                cd1.Members.OfType<FieldDeclarationSyntax>()
                        .Any(f => f.AttributeLists.Count>0));
            {
                ClassDeclarationSyntax cd = (ClassDeclarationSyntax)Node;
                if (cd.Members.OfType<IFieldSymbol>().Where(f =>
                        f.GetAttributes().Any(attr => attr.AttributeClass == typeof(ExposeDataAttr))).ToList().Count >
                    0)
                {
                    ClassDeclarations.Add(cd);
                    Console.WriteLine(
                        $"[ExposeDataGenerator] Added class declaration to ExposeDataGenerator: {cd.Identifier.Text}");
                }
            }
        }
    }


    [Generator]
    public class ExposeDataSourceGen : ISourceGenerator
    {
        #region System Quick Funcs.
        private void Log(string message)
        {
            Console.WriteLine($"[ExposeDataGenerator] {message}");
        }
        #endregion
        #region Warnings

        private static readonly DiagnosticDescriptor RuleClassNotPartial = new DiagnosticDescriptor(
            id: "EDG001",
            title: "Class must be partial to use Attr [ExposeDataAttr]",
            messageFormat: "Class {0} must be declared partial to use attr [ExposeDataAttr]",
            category: "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
        private static readonly DiagnosticDescriptor RuleExposeDataExists = new DiagnosticDescriptor(
            id: "EDG002",
            title: "ExposeData() already exists for this class",
            messageFormat: "Class {0} already contains a manually written ExposeData() method.this will cause conflicts when compiling with usage of attr [ExposeDataAttr]",
            category: "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        

        #endregion
        public void Initialize(GeneratorInitializationContext context)
        {
            Log("Initializing");
            context.RegisterForSyntaxNotifications(()=>new SyntaxRecv());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Log("Executing");
            SyntaxRecv recv = (SyntaxRecv)context.SyntaxReceiver;
            Log($"Found {recv.ClassDeclarations.Count} class declarations");
            //Find all fields with attr declared.
            foreach (var cd in recv.ClassDeclarations)
            {
                Log($"Processing class: {cd.Identifier.Text}");
                var model = context.Compilation.GetSemanticModel(cd.SyntaxTree);
                var classSymbol = ModelExtensions.GetDeclaredSymbol(model, cd);
                Log($"Found class symbol: {classSymbol.Name}");
                if (!cd.Modifiers.Any(SyntaxKind.PartialKeyword))
                {
                    Log($"Class {classSymbol.Name} is not partial. Issuing diagnostic.");
                    var diagnostic = Diagnostic.Create(RuleClassNotPartial, cd.Identifier.GetLocation(), classSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                    continue; // Skip code generation for this class
                }
                var methods = cd.Members.OfType<MethodDeclarationSyntax>();
                if (methods.Any(m => m.Identifier.Text == "ExposeData"))
                {
                    Log($"Class {classSymbol.Name} already contains a manually written ExposeData method. Issuing diagnostic.");
                    var diagnostic = Diagnostic.Create(RuleExposeDataExists, cd.Identifier.GetLocation(), classSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                    continue; // Skip code generation for this class
                }
                var fields = cd.Members.OfType<IFieldSymbol>().
                    Where(f=>f.GetAttributes().
                        Any(attr => attr.AttributeClass == typeof(ExposeDataAttr)));
                Log($"Found {fields.Count()} fields with [ExposeData] attribute");
                //Now generate the source.
                var sb = new StringBuilder();
                sb.AppendLine("public void ExposeData(){");
                foreach (var f in fields)
                {
                    sb.AppendLine($"Scribe_Values.Look(ref {f.Name},\"{f.Name}\" ");
                }

                sb.AppendLine("}");
                var cNS = classSymbol.ContainingNamespace.ToDisplayString();
                var cSGEN = new StringBuilder();
                cSGEN.AppendLine($"#{cNS}.{classSymbol.Name}.g.cs");
                cSGEN.AppendLine($"Generated at {DateTime.Now.ToString("F")}");
                cSGEN.AppendLine($"namespace {cNS} {{");
                cSGEN.AppendLine($"public partial class {classSymbol.Name} {{");
                cSGEN.AppendLine(sb.ToString());
                cSGEN.AppendLine("}}");
                Console.WriteLine(cSGEN.ToString());
            }
        }
    }
}