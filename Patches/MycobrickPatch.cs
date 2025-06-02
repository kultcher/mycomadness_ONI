using HarmonyLib;
using KMod;
using UnityEngine;
using TUNING;
using System.Collections.Generic;
using MycobrickMod.Elements;

namespace MycobrickMod.Patches
{
    public class ModMain : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("[MycobrickMod] Patching...");            
            //harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
    public static class MycobrickShroomLoadPatch
    {
        public static void Prefix()
        {
            Debug.Log("[MycobrickMod] Registering Mycobrick Shroom...");
            CROPS.CROP_TYPES.Add(new Crop.CropVal("MycofiberOre", 2700f, 300, true));

        }
    }
}