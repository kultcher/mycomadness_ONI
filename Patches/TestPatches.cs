using HarmonyLib;
using UnityEngine;
using MycobrickMod.Utils;
using MycobrickMod.Elements; // Assuming elements are here for context

namespace MycobrickMod.Patches
{
    [HarmonyPatch(typeof(KBatchedAnimController), nameof(KBatchedAnimController.SetSymbolTint))]
    public static class KBatchedAnimController_SetSymbolTint_Patch
    {
            
    }
}