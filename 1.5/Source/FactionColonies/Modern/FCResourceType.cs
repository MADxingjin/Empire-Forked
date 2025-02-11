using UnityEngine;
using Verse;

namespace FactionColonies.Modern{
    /// <summary>
    /// TODO Move ItemTitheThingFilter calc here.
    /// </summary>
    public class FCResource : IExposable
    {
        public readonly FCResourceTypeDef def;
        public string Name => def.label;
        public string Description => def.description;
        public ThingFilter ThingTitheFilter;
        public bool IsEnabled => def.techLevel <= Find.World.GetComponent<FactionFC>().techLevel;
        public double ProdBase, ProdMult, ProdOffset,ProdFlat;
        /// <summary>
        /// We no longer store offset in resource itself,instead now with a def as backend, event modifier in settlement stack can now iterate through FCResource.defs for match.
        /// </summary>
        public double ProdFinal => ((ProdBase + ProdOffset) * ProdMult)+ProdFlat;

        /// <summary>
        /// Base tithe rate,we directly get this from def.
        /// </summary>
        public double TitheBase => def.TitheBase;

        public double TitheMult, TitheOffset, TitheFlat;

        /// <summary>
        /// Final tithe rate of production in silver,this can goes beyond 1.(Intentional), if this val>1,Item tithe will have its itemcost > pay full in silver.
        /// </summary>
        public double TitheFinal => ((TitheBase + TitheOffset) * TitheMult) + TitheFlat;

        public double CostBase, CostMult, CostOffset, CostFlat;

        /// <summary>
        /// Cost in silver per worker assigned to this resource.
        /// </summary>
        public double CostFinal => ((CostBase + CostOffset) * CostMult) + CostFlat;

        public int WorkersAssigned, WorkersMax;

        public int TotalCost => Mathf.CeilToInt((float)CostFinal * WorkersAssigned);

        public void ExposeData()
        {

        }
    }
}