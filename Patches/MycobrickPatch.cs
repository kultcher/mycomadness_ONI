using HarmonyLib;
using KMod;
using UnityEngine;
using TUNING;
using System.Collections.Generic;
using MycobrickMod.Elements;
using MycobrickMod.Recipes;
using MycobrickMod.Utils;

namespace MycobrickMod.Patches
{
    public class ModMain : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("[MycobrickMod] Patching...");
            //harmony.PatchAll();
            foreach (var patch in Harmony.GetAllPatchedMethods())
{
            Debug.Log("Patched method: " + patch.FullDescription());
}
        }
    }

    [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
    public static class MycobrickShroomLoadPatch
    {
        public static void Prefix()
        {
            Debug.Log("[MycobrickMod] Registering Mycobrick Shroom...");
            CROPS.CROP_TYPES.Add(new Crop.CropVal("MycofiberElement", 100f, 300, true));
            KilnRecipes.AddMycobricksRecipe();
        }

        public static void Postfix()
        {

            ElementUtil.UpdateElementPrefabAnim(MycofiberElement.mycofiber_element_tag, MycofiberElement.substance);

            Debug.Log("[MycobrickMod] Assets.OnAllAssetsLoaded Postfix: Customizing default ore prefabs...");
        }
    }
    
}