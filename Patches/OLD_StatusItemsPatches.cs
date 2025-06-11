// using HarmonyLib;
// using UnityEngine;
// using System;
// using System.Collections.Generic;
// using MycobrickMod.Elements;
// using MycobrickMod.Patches; // for MycofiberSimHash
// using Database;


// namespace MycobrickMod.Patches
// {
//     [HarmonyPatch(typeof(CreatureStatusItems))]
//     [HarmonyPatch(MethodType.Constructor)]
//     [HarmonyPatch(new Type[] { typeof(ResourceSet) })]
//     public static class CreatureStatusItemsCtorPatch
//     {
//         public static void Postfix(CreatureStatusItems __instance)
//         {
//             CustomStatusItemsHelper.CreateCustomStatusItems(__instance);
//         }
//     }

//     public class CustomStatusItems
//     {
//         public static CreatureStatusItems producingPetroleum;
//     }


//     public static class CustomStatusItemsHelper
//     {
//         public static void CreateCustomStatusItems(CreatureStatusItems creatureStatusItems)
//         {
//                 creatureStatusItems.producingPetroleum = new StatusItem("ProducingPetroleum", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);//129022 is for all_overlays
//                 creatureStatusItems.producingPetroleum.resolveStringCallback = delegate(string str, object data)
//                 {
//                     var instance = (TarFunnelPlant.Instance)data;
//                     str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
//                     return str;
//                 };
//                 producingPetroleum.resolveTooltipCallback = (str, data) =>
//                 {
//                     var instance = (TarFunnelPlant.Instance)data;
//                     float fullness = instance.sm.Fullness.Get(instance);
//                     return $"Fluid fullness: {(fullness * 100f):F1}%";
//                 };
//                 producingPetroleum.resolveTooltipCallback = delegate(string str, object data)
//                 {
//                     TarFunnelPlant.Instance instance = (TarFunnelPlant.Instance)data;
//                     string text = str;
//                     string newValue = GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None);
//                     text = text.Replace("{0}", newValue);
                    
//                     str = str.Replace("{0}", GameUtil.GetFormattedMass(instance.GetProductionSpeed() * 20f / instance.OptimalProductionDuration, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
//                     return str;
//                 };
//                 var petroleumProductionPaused = new StatusItem("PetroleumProductionPaused", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
                
//                 petroleumProductionPaused.resolveStringCallback = delegate(string str, object data)
//                 {
//                     TarFunnelPlant.Instance instance = (TarFunnelPlant.Instance)data;
//                     str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
//                     return str;
//                 };
//                 petroleumProductionPaused.resolveTooltipCallback = delegate(string str, object data)
//                 {
//                     TarFunnelPlant.Instance instance = (TarFunnelPlant.Instance)data;
//                     return str;
//                 };
//                 creatureStatusItems.PetroleumProductionWilted = new StatusItem("PetroleumProductionWilted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
//                 creatureStatusItems.PetroleumProductionWilted.resolveStringCallback = delegate(string str, object data)
//                 {
//                     TarFunnelPlant.Instance instance = (TarFunnelPlant.Instance)data;
//                     str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
//                     return str;
//                 };      
//         }
//     }
// }