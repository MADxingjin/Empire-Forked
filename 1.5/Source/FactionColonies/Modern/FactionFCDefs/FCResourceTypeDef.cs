using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Verse;
using UnityEngine;
using RimWorld;
namespace FactionColonies {
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
        #endregion
    }
}