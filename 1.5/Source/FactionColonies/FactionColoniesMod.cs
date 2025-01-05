using System;
using System.Collections.Generic;
using FactionColonies.util;
using UnityEngine;
using Verse;

namespace FactionColonies
{
    public class FactionColoniesMod : Mod
    {
        public FactionColonies settings = new FactionColonies();

        public FactionColoniesMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<FactionColonies>();
        }

        string silverPerResource;
        string timeBetweenTaxes;
        string productionTitheMod;
        string workerCost;
        string settlementMaxLevel;
        int daysBetweenTaxes;
        IntRange minMaxDaysTillMilitaryAction = new IntRange(4, 10);
        IntRange minMaxDaysTillRandomEvent = new IntRange(0, 6);

        private Vector2 scrollVector = new Vector2();
        private float viewRectHeight = -1f;

        private bool firstRun = true;
        private bool fixDone = false;

        /// <summary>
        /// Creates an option for the list of ForcedTaxDeliveryOptions. Shuttles may not be used if royality is inactive
        /// </summary>
        private FloatMenuOption ShuttleOption
        {
            get
            {
                if (ModsConfig.RoyaltyActive)
                {
                    return new FloatMenuOption("taxDeliveryModeShuttleDesc".Translate(), delegate () {settings.forcedTaxDeliveryMode = TaxDeliveryMode.Shuttle;});
                }
                else 
                { 
                    return new FloatMenuOption("taxDeliveryModeShuttleUnavailableDesc".Translate(), null); 
                }
            }
        }

        /// <summary>
        /// Creates a list of options for forced tax delivery
        /// </summary>
        private List<FloatMenuOption> ForcedTaxDeliveryOptions
        {
            get
            {
                return new List<FloatMenuOption>() 
                {
                    new FloatMenuOption("taxDeliveryModeDefaultDesc".Translate(), delegate() {settings.forcedTaxDeliveryMode = default;}),
                    new FloatMenuOption("taxDeliveryModeTaxSpotDesc".Translate(), delegate() {settings.forcedTaxDeliveryMode = TaxDeliveryMode.TaxSpot;}),
                    new FloatMenuOption("taxDeliveryModeCaravanDesc".Translate(), delegate() {settings.forcedTaxDeliveryMode = TaxDeliveryMode.Caravan;}),
                    new FloatMenuOption("taxDeliveryModeDropPodDesc".Translate(), delegate() {settings.forcedTaxDeliveryMode = TaxDeliveryMode.DropPod;}),
                    ShuttleOption
                };
            }
        }

/// <summary>
/// Draw that intensive Settings Menu.
/// </summary>
/// <param name="inRect"></param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            silverPerResource = settings.silverPerResource.ToString();
            timeBetweenTaxes = (settings.timeBetweenTaxes / 60000).ToString();
            productionTitheMod = settings.productionTitheMod.ToString();
            workerCost = settings.workerCost.ToString();
            settlementMaxLevel = settings.settlementMaxLevel.ToString();
            daysBetweenTaxes = settings.timeBetweenTaxes / 60000;

            minMaxDaysTillMilitaryAction = new IntRange(settings.minDaysTillMilitaryAction, settings.maxDaysTillMilitaryAction);
            minMaxDaysTillRandomEvent = new IntRange(settings.minDaysTillRandomEvent, settings.maxDaysTillRandomEvent);

            viewRectHeight = viewRectHeight == -1f ? float.MaxValue : viewRectHeight;
            Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width - 17f, viewRectHeight);

            Widgets.BeginScrollView(inRect, ref scrollVector, viewRect);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(viewRect);
            ls.Label("FCSettingSilverPerResource".Translate());
            ls.IntEntry(ref settings.silverPerResource, ref silverPerResource);
            ls.Label("FCSettingDaysBetweenTax".Translate());
            ls.IntEntry(ref daysBetweenTaxes, ref timeBetweenTaxes);
            settings.timeBetweenTaxes = Math.Max(1, daysBetweenTaxes) * 60000;
            ls.Label("FCSettingProductionTitheMod".Translate());
            ls.IntEntry(ref settings.productionTitheMod, ref productionTitheMod);
            ls.Label("FCSettingWorkerCost".Translate());
            ls.IntEntry(ref settings.workerCost, ref workerCost);
            ls.Label("FCSettingMaxSettlementLevel".Translate());
            ls.IntEntry(ref settings.settlementMaxLevel, ref settlementMaxLevel);
            ls.CheckboxLabeled("MedievalTechOnly".Translate(), ref settings.medievalTechOnly);
            ls.CheckboxLabeled("FCSettingDisableHostileMilActions".Translate(), ref settings.disableHostileMilitaryActions);
            ls.CheckboxLabeled("FCSettingDisableRandomEvents".Translate(), ref settings.disableRandomEvents);
            ls.CheckboxLabeled("FCSettingDeadPawnsIncreaseMilCooldown".Translate(), ref settings.deadPawnsIncreaseMilitaryCooldown);
            ls.CheckboxLabeled("FCSettingForcedPausing".Translate(), ref settings.disableForcedPausingDuringEvents);
            //ls.CheckboxLabeled("FCSettingAutoResolveBattles".Translate(), ref settings.settlementsAutoBattle);
            if (ls.ButtonText("selectTaxDeliveryModeButton".Translate() + settings.forcedTaxDeliveryMode)) Find.WindowStack.Add(new FloatMenu(ForcedTaxDeliveryOptions));

            ls.Label("FCSettingMinMaxMilitaryAction".Translate());
            ls.IntRange(ref minMaxDaysTillMilitaryAction, 1, 30);
            settings.minDaysTillMilitaryAction = minMaxDaysTillMilitaryAction.min;
            settings.maxDaysTillMilitaryAction = Math.Max(1, minMaxDaysTillMilitaryAction.max);

            ls.Label("FCSettingMinMaxRandomEvent".Translate());
            ls.IntRange(ref minMaxDaysTillRandomEvent, 0, 30);
            settings.minDaysTillRandomEvent = minMaxDaysTillRandomEvent.min;
            settings.maxDaysTillRandomEvent = Math.Max(1, minMaxDaysTillRandomEvent.max);

            if (ls.ButtonText("FCOpenPatchNotes".Translate())) DebugActionsMisc.PatchNotesDisplayWindow();

            if (ls.ButtonText("FCSettingResetButton".Translate()))
            {
                FactionColonies blank = new FactionColonies();
                settings.silverPerResource = blank.silverPerResource;
                settings.timeBetweenTaxes = blank.timeBetweenTaxes;
                settings.productionTitheMod = blank.productionTitheMod;
                settings.workerCost = blank.workerCost;
                settings.medievalTechOnly = blank.medievalTechOnly;
                settings.settlementMaxLevel = blank.settlementMaxLevel;
                settings.minDaysTillMilitaryAction = blank.minDaysTillMilitaryAction;
                settings.maxDaysTillMilitaryAction = blank.maxDaysTillMilitaryAction;
                settings.minDaysTillRandomEvent = blank.minDaysTillRandomEvent;
                settings.maxDaysTillRandomEvent = blank.maxDaysTillRandomEvent;
                settings.disableRandomEvents = blank.disableRandomEvents;
                settings.deadPawnsIncreaseMilitaryCooldown = blank.deadPawnsIncreaseMilitaryCooldown;
                settings.settlementsAutoBattle = blank.settlementsAutoBattle;
                settings.disableForcedPausingDuringEvents = blank.disableForcedPausingDuringEvents;
                settings.forcedTaxDeliveryMode = blank.forcedTaxDeliveryMode;
            }

            FixScrollingBug(ls);
            ls.End();

            Widgets.EndScrollView();
            base.DoSettingsWindowContents(inRect);
        }

        private void FixScrollingBug(Listing_Standard ls)
        {
            if (fixDone) return;

            if (!firstRun)
            {
                viewRectHeight = ls.CurHeight + 5f;
                fixDone = true;
            }
            else
            {
                viewRectHeight = float.MaxValue;
                firstRun = false;
            }
        }

        public override string SettingsCategory()
        {
            return "Empire";
        }

        public override void WriteSettings()
        {
            LoadedModManager.GetMod<FactionColoniesMod>().GetSettings<FactionColonies>().timeBetweenTaxes = daysBetweenTaxes * 60000;
            base.WriteSettings();
        }
    }
}