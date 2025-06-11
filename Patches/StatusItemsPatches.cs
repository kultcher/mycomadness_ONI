using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using Database; // For Db.Get() if you need it later, not directly for StatusItem creation
using STRINGS;

namespace MycobrickMod.Patches
{
    public static class ModStatusItems
    {
        public static StatusItem ProducingPetroleum;
        public static StatusItem PetroleumProductionPaused;
        public static StatusItem PetroleumProductionWilted;
    }

    [HarmonyPatch(typeof(CreatureStatusItems))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(ResourceSet) })]
    public static class CreatureStatusItemsCtorPatch
    {
        public static void Postfix(CreatureStatusItems __instance) 
        {
            ModStatusItemsInitializer.CreateCustomStatusItems();
        }
    }

    public static class ModStatusItemsInitializer
    {
        public static void CreateCustomStatusItems()
        {
            ModStatusItems.ProducingPetroleum = new StatusItem(
                id: "Myco_ProducingPetroleum",
                name: "Producing Petroleum", // TODO: Put this in STRINGS
                tooltip: "This Tar Funnel is currently producing petroleum.\nFullness: {0}", // TODO: Put this in STRINGS
                icon: "Petroleum",
                icon_type: StatusItem.IconType.Info,
                notification_type: NotificationType.Good,
                allow_multiples: false,
                render_overlay: OverlayModes.None.ID
            );

            ModStatusItems.ProducingPetroleum.resolveStringCallback = (string_str, data) =>
            {
                if (data is TarFunnelPlant.Instance instance)
                {
                    return $"Producing Petroleum ({GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None)} Full)";
                }
                else
                {
                    Debug.LogWarning($"[TarFunnel] ProducingPetroleum resolveStringCallback - Data is NOT TarFunnelPlant.Instance. String was: {string_str}");
                }
                return string_str;
            };

            ModStatusItems.ProducingPetroleum.resolveTooltipCallback = (tooltip_str, data) =>
            {
                if (data is TarFunnelPlant.Instance instance)
                {
                    // String defined in StatusItem constructor will be tooltip_str
                    tooltip_str = tooltip_str.Replace("ProducingPetroleum: {0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
                    tooltip_str += $"\nProduction Rate: {GameUtil.GetFormattedMass(instance.GetProductionSpeed() * 10f / instance.OptimalProductionDuration, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.##}")}/cycle"; // ** 10f?
                    return tooltip_str;
                }
                return tooltip_str;
            };

            // Initialize PetroleumProductionPaused
            ModStatusItems.PetroleumProductionPaused = new StatusItem(
                id: "PetroleumProductionPaused",
                name: "Petroleum Production Paused", // TODO: STRINGS
                tooltip: "Petroleum production is paused.\nFullness: {0}", // TODO: STRINGS
                icon: "Petroleum",
                icon_type: StatusItem.IconType.Info,
                notification_type: NotificationType.BadMinor,
                allow_multiples: false,
                render_overlay: OverlayModes.None.ID
            );

            ModStatusItems.PetroleumProductionPaused.resolveStringCallback = (string_str, data) =>
            {
                if (data is TarFunnelPlant.Instance instance)
                {
                    //return string_str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
                    return $"Production Paused ({GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None)} Full)"; // Example
                }
                return string_str;
            };
            ModStatusItems.PetroleumProductionPaused.resolveTooltipCallback = (tooltip_str, data) =>
            {
                 if (data is TarFunnelPlant.Instance instance)
                {
                    return tooltip_str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
                    // Add reasons for pause if available from instance, e.g.
                    // tooltip_str += "\nReason: Internal storage full.";
                    // tooltip_str += "\nReason: Insufficient Crude Oil.";
                }
                return tooltip_str;
            };


            // Initialize PetroleumProductionWilted
            ModStatusItems.PetroleumProductionWilted = new StatusItem(
                id: "PetroleumProductionWilted",
                name: "Petroleum Production Wilted", // TODO: STRINGS
                tooltip: "The Tar Funnel is wilted and cannot produce petroleum.", // TODO: STRINGS
                icon: "Petroleum",
                icon_type: StatusItem.IconType.Info,
                notification_type: NotificationType.BadMinor,
                allow_multiples: false,
                render_overlay: OverlayModes.None.ID
            );

            ModStatusItems.PetroleumProductionWilted.resolveStringCallback = (string_str, data) =>
            {
                // if (data is TarFunnelPlant.Instance instance)
                // {
                //     // No dynamic text needed for the main string usually if it's just "Wilted"
                // }
                return "Production Wilted"; // Example
            };
            // resolveTooltipCallback for wilted is usually just the static string from constructor
        }
    }
}