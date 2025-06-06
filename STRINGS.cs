using STRINGS;

namespace MycobrickMod
{
    public class STRINGS
    {
        public class ITEMS
        {
            public class INDUSTRIAL_PRODUCTS
            {
                public class MYCOWEAVESTRANDS
                {
                    public static LocString NAME = UI.FormatAsLink("Mycoweave Strands", "MYCOWEAVESTRANDS");
                    public static LocString DESC = "Treated and processed Mycofiber strands that can be woven into durable fabrics and construction materials.";
                }
            }
        }
        public class CREATURES
        {
            public class SPECIES
            {
                public class MYCOBRICKSHROOM
                {
                    public static LocString NAME = "Mycobrick Shroom";
                    public static LocString DESC = "A robust fungal plant with a thick stalk that can be processed into durable organic bricks.";
                    public static LocString DOMESTICATEDDESC = "This engineered mushroom thrives in dense atmospheres and can rapidly grow on basic fertilizer.";
                }

                public class SEEDS
                {
                    public class MYCOBRICKSHROOMSEED
                    {
                        public static LocString NAME = "Mycobrick Spore";
                        public static LocString DESC = "A hardy spore from a Mycobrick Shroom. Can be planted in a farm tile or natural soil.";
                    }
                }
            }
        }

        public class BUILDINGS
        {
            public class PREFABS
            {
                public class KILN
                {
                    public static LocString MYCOBRICK_RECIPE_DESC = "Bake raw mycofiber into durable Mycobricks.";
                }

                public class SLUDGEPRESS
                {
                    public static LocString MYCOWEAVE_RECIPE_DESC = "Process Mycofiber with water and rot pile to create durable Mycoweave Strands.";
                }
                public class MYCOFIBERTILE
                {
                    public static LocString NAME = "Mycobfiber Tile";
                    public static LocString DESC = "A soft tile made of squishy, fibrous Mycofiber. Quick to build, but fragile and tough to walk on.";
                    public static LocString EFFECT = "A soft tile made of squishy, fibrous Mycofiber.";
                }
            }
        }

        public class CODEX
        {
            public class MYCOWEAVESTRANDS
            {
                // Token: 0x04009C06 RID: 39942
                public static LocString TITLE = "Mycoweave Strands";

                // Token: 0x04009C07 RID: 39943
                public static LocString SUBTITLE = "Textile Ingredient";

                // Token: 0x02002B88 RID: 11144
                public class BODY
                {
                    // Token: 0x0400BFB2 RID: 49074
                    public static LocString CONTAINER1 = string.Concat(new string[]
                    {
                            "A cleaned and processed bundle of Mycofiber from the ",
                            UI.FormatAsLink("Mycobrick Shroom", "MYCOBRICKSHROOM"),
                            ".\n\nIt is used in the production of ",
                            UI.FormatAsLink("Clothing", "EQUIPMENT"),
                            " and textiles."
                    });
                }
            }
        }
    }
}
