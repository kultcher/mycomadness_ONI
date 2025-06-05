using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using MycobrickMod.Elements;
using MycobrickMod.Patches; // for MycofiberSimHash

[HarmonyPatch(typeof(Pickupable), "OnPrefabInit")]
public static class Pickupable_OnSpawn_Patch
{
    public static void Postfix(Pickupable __instance)
    {
        PrimaryElement elem = __instance.GetComponent<PrimaryElement>();
        if (elem == null) return;
        var anim = __instance.GetComponent<KBatchedAnimController>();
        var symbol = new KAnimHashedString("ui");

        // Check if this is a drop of Mycofiber
        if (elem.ElementID == MycofiberElement.MycofiberSimHash)
        {
            anim.TintColour = MycofiberElement.MYCOFIBER_COLOR;
            if (anim != null)
            {
                anim.SetSymbolTint(symbol, MycofiberElement.MYCOFIBER_COLOR);
            }
        }
        if (elem.ElementID == MycobrickElement.MycobrickSimHash)
        {
            anim.TintColour = MycobrickElement.MYCOBRICK_COLOR;
            if (anim != null)
            {
                // Try common symbol name(s) for drop appearance
                anim.SetSymbolTint(symbol, MycobrickElement.MYCOBRICK_COLOR);
            }
        }
    }
}