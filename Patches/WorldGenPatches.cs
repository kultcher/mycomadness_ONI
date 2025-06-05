using HarmonyLib;
using Klei; // For YamlIO if you were to load settings from YAML (not needed here)
using ProcGen;
using System.Collections.Generic;
using UnityEngine; // For Debug.Log
using MycobrickMod.WorldGen; // To access MycobrickShroomGenTuning


namespace MycobrickMod.Patches
{
    public static class WorldGenPatches
    {
        // This dictionary will hold our plant's ID and its tuning.
        // For a single plant, it's simple. For multiple, it's more useful.
        private static Dictionary<string, PlantGenTuning.PlantTuning> PlantsToAddToWorldGen =
            new Dictionary<string, PlantGenTuning.PlantTuning>();

        // Call this from your ModMain.OnLoad or a Db.Initialize Postfix to set up the dictionary
        public static void InitializePlantGenSettings()
        {
            // MycobrickShroomConfig.ID should be "MycobrickShroom"
            PlantsToAddToWorldGen[MycobrickShroomConfig.ID] = PlantGenTuning.MycobrickShroomTuning;
            Debug.Log($"[MycobrickMod] InitializePlantGenSettings: Prepared '{MycobrickShroomConfig.ID}' for worldgen.");
        }

 [HarmonyPatch(typeof(SettingsCache), "LoadFiles", new System.Type[] { typeof(string), typeof(string), typeof(List<YamlIO.Error>) })]
        public static class SettingsCache_LoadFiles_Patch
        {
            public static void Postfix()
            {
                if (SettingsCache.mobs == null || SettingsCache.mobs.MobLookupTable == null)
                {
                    Debug.LogWarning("[MycobrickMod] SettingsCache.mobs.MobLookupTable is null. Cannot add custom plants to worldgen mobs.");
                    return;
                }

                Debug.Log("[MycobrickMod] SettingsCache_LoadFiles_Patch.Postfix: Adding custom plants to MobLookupTable.");
                foreach (var entry in PlantsToAddToWorldGen) // PlantsToAddToWorldGen is your dictionary
                {
                    string plantId = entry.Key; // e.g., "MycobrickShroom"
                    PlantGenTuning.PlantTuning tuning = entry.Value;

                    if (!SettingsCache.mobs.MobLookupTable.ContainsKey(plantId))
                    {
                        // Create a new Mob entry for worldgen
                        Mob plantMobEntry = new Mob(tuning.spawnLocation) // Set spawn location via constructor
                        {
                            name = plantId // 'name' is usually a public field or settable property
                            // numToSpawn is often left null if density is used.
                            // mobTags can be initialized if needed, or left as default (often null or empty HashSet)
                        };
                        // plantMobEntry.mobTags = new HashSet<Tag>(); // If you need to initialize/set it

                        // Use Traverse to set properties that might not be publicly settable
                        Traverse mobTraverse = Traverse.Create(plantMobEntry);

                        //mobTraverse.Property("width", null).SetValue(1);
                        //mobTraverse.Property("height", null).SetValue(1);
                        //mobTraverse.Property("density", null).SetValue(tuning.density);
                        SettingsCache.mobs.MobLookupTable.Add(plantId, plantMobEntry);
                        
                        // Attempt to set width, height, density using Traverse
                        // The .Property("name", null) syntax usually means find a property OR field with that name.
                        // If they are indeed fields, .Field("name") is more direct.
                        // Let's assume they might be properties first, as in the example.
                        mobTraverse.Property("width").SetValue(tuning.width);
                        mobTraverse.Property("height").SetValue(tuning.height);
                        mobTraverse.Property("density").SetValue(tuning.density); 
                        // mobTraverse.Property("numToSpawn").SetValue(null); // Explicitly null if desired, though often default is fine.
                        // mobTraverse.Property("mobTags").SetValue(new HashSet<Tag> { GameTags.Plant }); // Example

                        Debug.Log($"[MycobrickMod] Added '{plantId}' to SettingsCache.mobs.MobLookupTable. Width: {tuning.width}, Height: {tuning.height}, Density: {tuning.density.min}-{tuning.density.max}");
                    }
                    else
                    {
                        Debug.Log($"[MycobrickMod] '{plantId}' already exists in SettingsCache.mobs.MobLookupTable. Skipping.");
                    }
                }
            }
        }
        [HarmonyPatch(typeof(SettingsCache), "LoadSubworlds")]
        public static class SettingsCache_LoadSubworlds_Patch
        {
            public static void Postfix()
            {
                if (SettingsCache.subworlds == null)
                {
                    Debug.LogWarning("[MycobrickMod] SettingsCache.subworlds is null. Cannot add custom plants to biomes.");
                    return;
                }

                foreach (SubWorld subworld in SettingsCache.subworlds.Values)
                {
                    if (subworld.biomes == null) continue;

                    foreach (WeightedBiome biome in subworld.biomes)
                    {
                        foreach (var entry in PlantsToAddToWorldGen)
                        {
                            string plantId = entry.Key; // e.g., "MycobrickShroom"
                            PlantGenTuning.PlantTuning tuning = entry.Value;

                            if (tuning.IsValidBiome(subworld, biome.name)) // biome.name is like "subworlds/forest/ForestBiome"
                            {
                                if (biome.tags == null) // Initialize if null
                                {
                                    // Use Traverse to set private field if necessary, or hope it's a public List/HashSet
                                    // For simplicity, let's assume it's a List that can be modified.
                                    // In ONI, biome.tags is often a List<string> or similar.
                                    // If it's private, you might need Traverse.Create(biome).Field("tags").GetValue<List<string>>()
                                    // and then set it back if it was null and you created a new list.
                                    // However, often it's already initialized.
                                    // The MoreCuisineVariety example uses Traverse if it's null.
                                    // For now, let's try direct access and see if it crashes (it might be a property with a public setter).
                                    // biome.tags = new List<string>(); // This might be wrong if it's not a settable property
                                    // A safer way if it might be null and you can't directly assign new:
                                    var tagsList = Traverse.Create(biome).Property<List<string>>("tags").Value;
                                    if (tagsList == null)
                                    {
                                        tagsList = new List<string>();
                                        Traverse.Create(biome).Property<List<string>>("tags").Value = tagsList;
                                        Debug.Log("[MycobrickMod] SettingsCache_LoadSubworlds_Patch.Postfix: Adding custom plants to subworld biomes.");
                                    }
                                }

                                if (!biome.tags.Contains(plantId))
                                {
                                    biome.tags.Add(plantId); // Add plant ID (e.g., "MycobrickShroom") to biome's spawnable tags
                                    Debug.Log($"[MycobrickMod] Added '{plantId}' to biome '{biome.name}' in subworld '{subworld.name}'.");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}