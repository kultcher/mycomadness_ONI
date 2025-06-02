// MycobrickMod/WorldGen/PlantGenTuning.cs (New File)
using ProcGen;
using System.Collections.Generic;
using System.Linq; // For Any() if you use it

namespace MycobrickMod.WorldGen // Or your preferred namespace
{
    public static class PlantGenTuning
    {

        public static class BiomesStrings
        {
            public static string PREFIX = "biomes/";
            public static string BARREN = "Barren";
            public static string DEFAULT = (PREFIX + "Default");
            public static string FOREST = (PREFIX + "Forest");
            public static string FROZEN = (PREFIX + "Frozen");
            public static string MARSH = (PREFIX + "HotMarsh");
            public static string JUNGLE = (PREFIX + "Jungle");
            public static string MAGMA = (PREFIX + "Magma");
            public static string MISC = (PREFIX + "Misc");
            public static string EMPTY = (PREFIX + "Misc/Empty");
            public static string OCEAN = (PREFIX + "Ocean");
            public static string OIL = (PREFIX + "Oil");
            public static string RUST = (PREFIX + "Rust");
            public static string SEDIMENTARY = (PREFIX + "Sedimentary");
            public static string AQUATIC = (PREFIX + "Aquatic");
            public static string METALLIC = (PREFIX + "Metallic");
            public static string MOO = (PREFIX + "Moo");
            public static string NIOBIUM = (PREFIX + "Niobium");
            public static string RADIOACTIVE = (PREFIX + "Radioactive");
            public static string SWAMP = (PREFIX + "Swamp");
            public static string WASTELAND = (PREFIX + "Wasteland");
        }

        public class PlantTuning
        {
            public MinMax density; // e.g., new MinMax(0.01f, 0.05f) for fairly rare
            public ISet<Temperature.Range> biomeTemperatures;
            public ISet<string> biomes; // List of biome ID strings
            public Mob.Location spawnLocation;
            public int width;
            public int height;

            // Helper to check if this plant can spawn in a given subworld's biome
            public bool IsValidBiome(SubWorld subworld, string biomeNameFromProcGen)
            {
                // biomeNameFromProcGen is usually like "subworlds/forest/ForestBiome"
                // Your this.biomes should contain "subworlds/forest" or a part that biomeNameFromProcGen.Contains()
                if (this.biomes == null || !this.biomes.Any(b => biomeNameFromProcGen.Contains(b)))
                {
                    return false;
                }
                if (this.biomeTemperatures == null || !this.biomeTemperatures.Contains(subworld.temperatureRange))
                {
                    return false;
                }
                return true;
            }
        }

        public static PlantTuning MycobrickShroomTuning { get; private set; } // Changed name for clarity, made setter private


        static PlantGenTuning()
        {
            MycobrickShroomTuning = new PlantTuning
            {
                density = new MinMax(0.25f, 0.50f), // Adjust as needed
                biomeTemperatures = new HashSet<Temperature.Range>
                {
                    //Temperature.Range.Cold, // Example: If it likes cold areas
                    Temperature.Range.Mild,
                    Temperature.Range.Room,
                    Temperature.Range.HumanWarm,
                    //Temperature.Range.Tropical, // Example
                },
                biomes = new HashSet<string>
                {
                    BiomesStrings.DEFAULT,
                    BiomesStrings.SWAMP,
                    "subworlds/Default",
                },
                spawnLocation = Mob.Location.Floor, // Your shroom grows on the floor
                width = 3, // From your MycobrickShroomConfig
                height = 3  // From your MycobrickShroomConfig
            };
        }
    }
}