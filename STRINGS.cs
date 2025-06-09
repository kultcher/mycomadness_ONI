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
                    public static LocString DESC = "The first stage of Mycofiber processing involves introducing a binding agent before violently removing impurities.";
                }
    
                public class MYCOWEAVEMATTING
                {
                    public static LocString NAME = UI.FormatAsLink("Mycoweave Matting", "MYCOWEAVEMATTING");
                    public static LocString DESC = "Further processing and disinfection renders Mycofiber strands into a flexible and functional textile.";
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
                public static LocString TITLE = "Mycoweave Strands";
                public static LocString SUBTITLE = "Textile Ingredient";
                public class BODY
                {
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
            public class MYCOWEAVEMATTING
            {
                public static LocString TITLE = "Mycoweave Matting";
                public static LocString SUBTITLE = "Textile Ingredient";
                public class BODY
                {
                    public static LocString CONTAINER1 = string.Concat(new string[]
                    {
                            "Further processed of Mycoweave strands from the ",
                            UI.FormatAsLink("Mycobrick Shroom", "MYCOBRICKSHROOM"),
                            " render them into a flexible textile.\n\nIt is used in the production of ",
                            UI.FormatAsLink("Clothing", "EQUIPMENT"),
                            " and textiles."
                    });
                }
            }
        }

        public class EQUIPMENT
        {
            public class PREFABS
            {
                public class MYCOWEAVE_LUNGSUIT
                {
                    public static LocString NAME = "Mycoweave Lungsuit";
                    public static LocString DESC = "A lightweight vest woven from Mycoweave Strands that enhances respiratory efficiency.";
                    public static LocString RECIPE_DESC = "Create a durable vest from Mycoweave Strands.";
                    public static LocString EFFECT = "Placeholder";
                }
            }
        }
    }
}
