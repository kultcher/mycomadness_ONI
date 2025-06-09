using HarmonyLib;
using KMod;
using UnityEngine;
using TUNING;
using System;
using MycobrickMod.Elements;
using MycobrickMod.Recipes;
using MycobrickMod.Utils;
using STRINGS;

namespace MycobrickMod.Patches
{
    public class ModMain : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Debug.Log("[MycobrickMod] Patching...");
            foreach (var patch in Harmony.GetAllPatchedMethods())
            {
                Debug.Log("Patched method: " + patch.FullDescription());
            }
        }
    }

    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            MycoweaveStrandsConfig.RegisterStrings();
            MycoweaveLungsuitConfig.RegisterStrings();
            MycoweaveMattingConfig.RegisterStrings();

            MycobrickRecipe.AddMycobricksRecipe();
            MycoweaveRecipes.AddMycoweaveRecipes();

            // if (!GameTags.Fabrics.Contains(MycoweaveStrandsConfig.TAG))
            // {
            //     List<Tag> fabricsList = GameTags.Fabrics.ToList();
            //     fabricsList.Add(MycoweaveStrandsConfig.TAG);
            //     GameTags.Fabrics = fabricsList.ToArray();

            //     Debug.Log($"[MycobrickMod] Added {MycoweaveStrandsConfig.TAG} to GameTags.Fabrics array. Total fabrics: {GameTags.Fabrics.Length}");
            // }
        }





        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public static class MycobrickShroomLoadPatch
        {
            public static void Prefix()
            {
                Debug.Log("[MycobrickMod] Registering Mycobrick Shroom and items...");
                CROPS.CROP_TYPES.Add(new Crop.CropVal("MycofiberElement", 100f, 300, true));
                new MycoweaveStrandsConfig().CreatePrefab();

            }
            public static void Postfix()
            {
                Debug.Log("[MycobrickMod] Main patch postfix.");
            }
        }
    }
}