using HarmonyLib;
using System.IO; // For Path.Combine
using Klei; // For YamlIO
using UnityEngine;
namespace MycobrickMod
{
    [HarmonyPatch(typeof(CodexCache), "CodexCacheInit")]
    public static class CodexCache_CodexCacheInit_Debug_Patch
    {
        public static void Prefix()
        {
            Debug.Log($"[MycobrickMod] CodexCache.CodexCacheInit_Debug_Patch Prefix: Checking for '{MycofiberOreConfig.ID}' prefab in Assets.");
            Tag oreTag = MycofiberOreConfig.MYCOFIBER_ORE_TAG; // This is TagManager.Create("MycofiberOre")
            GameObject orePrefab = Assets.GetPrefab(oreTag);
            if (orePrefab != null)
            {
                Debug.Log($"[MycobrickMod] CodexCache.CodexCacheInit_Debug_Patch: '{MycofiberOreConfig.ID}' prefab FOUND in Assets just before Codex generation. GameObject name: {orePrefab.name}");
            }
            else
            {
                Debug.LogWarning($"[MycobrickMod] CodexCache.CodexCacheInit_Debug_Patch: '{MycofiberOreConfig.ID}' (Tag to search: {oreTag.Name}, Hash: {oreTag.GetHash()}) prefab NOT FOUND in Assets at this point!");

                // Optional: Dump all prefabs if not found
                // Debug.LogWarning($"[MycobrickMod] Listing all prefabs in Assets.Prefabs ({Assets.Prefabs.Count} total):");
                // foreach(KeyValuePair<Tag, KPrefabID> entry in Assets.Prefabs)
                // {
                //    Debug.Log($"Assets - KeyTag: {entry.Key.Name} (Hash:{entry.Key.GetHash()}), ValueGO: {entry.Value.gameObject.name}, ValueTag: {entry.Value.GetTag().Name}");
                // }
            }

            // Also check if the CropType is registered
            if (TUNING.CROPS.CROP_TYPES.Exists(ct => ct.cropId == MycofiberOreConfig.ID))
            {
                Debug.Log($"[MycobrickMod] CodexCache.CodexCacheInit_Debug_Patch: CropType '{MycofiberOreConfig.ID}' IS registered in TUNING.CROPS.CROP_TYPES.");
            }
            else
            {
                Debug.LogWarning($"[MycobrickMod] CodexCache.CodexCacheInit_Debug_Patch: CropType '{MycofiberOreConfig.ID}' IS NOT registered in TUNING.CROPS.CROP_TYPES. This will cause issues for the Crop component.");
            }
        }
    }
}