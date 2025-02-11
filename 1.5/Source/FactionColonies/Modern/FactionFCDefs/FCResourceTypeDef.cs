using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Verse;
using UnityEngine;
using RimWorld;
namespace FactionColonies.Modern {
    /// <summary>
    /// 
    /// </summary> 
    public class FCResourceTypeDef :Def{
        #region Params
        /// <summary>
        /// At which tech level this resource will appear in settlement`s list and can be assigned to workers or passive generation buildings as target
        /// </summary>
        public TechLevel techLevel = TechLevel.Animal;

        /// <summary>
        /// A starting value for default tithe rate for this type of resource, if not specified(which is most resource, 0)
        /// </summary>
        public double TitheBase = 0d;

/// <summary>
/// The thingcategories this resource type supports to become thing tithe.
/// </summary>
        public List<ThingCategoryDef> TitheFilterCategories = null;
        /// <summary>
        /// The things this resource type supports to become thing tithe;
        /// </summary> 
        public List<ThingDef> TitheFilterSpecific = null;

        /// <summary>
        /// For filtering out a specific sub category from bigger one.
        /// </summary>
        public List<ThingCategoryDef> TitheFilterCategoriesDenylist = null;
        /// <summary>
        /// For filtering a specific thing from the pool.
        /// </summary>
        public List<ThingDef> TitheFilterSpecificDenylist = null;

/// <summary>
/// For those trick needs, maybe useful later on. Add stuff thing to thing tithe filter based on give Stuffcategories.
/// </summary>
        public List<StuffCategoryDef> TitheFilterStuffCategories = null;

        public Dictionary<ThingCategoryDef,TechLevel> TechlockedTitheFilterCategories = null;
        public Dictionary <ThingDef,TechLevel> TechlockedTitheFilterSpecific =null;
        [Unsaved(false)]
        private HashSet<ThingDef> allAlwaysAvailableThingDefsCached;
        #endregion

        #region Func

        public override void ResolveReferences()
        {
            allAlwaysAvailableThingDefsCached = new HashSet<ThingDef>();
            foreach(var tfc in TitheFilterCategories){
            foreach(var t in tfc.childThingDefs){
                allAlwaysAvailableThingDefsCached.Add(t);
            }
            }
            foreach(var t in TitheFilterSpecific){
                allAlwaysAvailableThingDefsCached.Add(t);
            }
            foreach(var tfcd in TitheFilterCategoriesDenylist){
                foreach(var td in tfcd.childThingDefs){
                    allAlwaysAvailableThingDefsCached.Remove(td);
                }
            }
            foreach(var td in TitheFilterSpecificDenylist){
                allAlwaysAvailableThingDefsCached.Remove(td);
            }
            //Our fast list is perpared and ready to use.
        }

        #endregion
    }
}