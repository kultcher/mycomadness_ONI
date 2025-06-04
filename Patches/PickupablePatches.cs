// using HarmonyLib;
// using UnityEngine;
// using KSerialization;
// using MycobrickMod.Elements; // for MycofiberSimHash

// [HarmonyPatch(typeof(Pickupable), "OnSpawn")]
// public static class Pickupable_OnSpawn_Patch
// {
//     public static void Postfix(Pickupable __instance)
//     {
//         PrimaryElement elem = __instance.GetComponent<PrimaryElement>();
//         if (elem == null) return;

//         // Check if this is a drop of Mycofiber
//         if (elem.ElementID == MycofiberElement.MycofiberSimHash)
//         {
//             var anim = __instance.GetComponent<KBatchedAnimController>();
//             if (anim != null)
//             {
//                 // Try common symbol name(s) for drop appearance
//                 var symbol = new KAnimHashedString("object");
//                 var tintColor = MycofiberElement.MYCOFIBER_COLOR;
//                 anim.SetSymbolTint(symbol, (Color)tintColor);

//                 symbol = new KAnimHashedString("ui");
//                 anim.SetSymbolTint(symbol, (Color)tintColor);

//             }
//         }
//         if (elem.ElementID == MycobrickElement.MycobrickSimHash)
//         {
//             var anim = __instance.GetComponent<KBatchedAnimController>();
//             if (anim != null)
//             {
//                 // Try common symbol name(s) for drop appearance
//                 var symbol = new KAnimHashedString("cinnabar");
//                 var tintColor = MycofiberElement.MYCOFIBER_COLOR;

//                 anim.SetSymbolTint(symbol, (Color)tintColor);
//             }
//         }
//     }
// }