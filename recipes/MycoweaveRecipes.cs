using System.Collections.Generic;
using System;

namespace MycobrickMod.Recipes;

public static class MycoweaveRecipes
{
    public static void AddMycoweaveRecipes()
    {
        AddMycoweaveStrandsRecipe();
        AddMycoweaveMattingRecipe();
        AddMycoweaveLungsuitRecipe();
        //AddMycoweaveWarmVestRecipe();
    }

    public static void AddMycoweaveStrandsRecipe()
        {
            Debug.Log("[MycobrickMod] Adding MycoweaveStrands recipe...");

            float mycofiberInput = 100f;
            float waterInput = 50f;
            float rotPileInput = 5f;
            float mycoweaveOutput = 1f;
            float pollutedWaterOutput = 50f;

            ComplexRecipe.RecipeElement[] mycoweaveRecipeInputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(MycobrickMod.Elements.MycofiberElement.MycofiberSimHash.CreateTag(), mycofiberInput),
                new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), waterInput),
                //new ComplexRecipe.RecipeElement(TagManager.Create(RotPileConfig.ID), rotPileInput)            
            };

            ComplexRecipe.RecipeElement[] mycoweaveRecipeOutputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(MycoweaveStrandsConfig.TAG, mycoweaveOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
                new ComplexRecipe.RecipeElement(SimHashes.DirtyWater.CreateTag(), pollutedWaterOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)            };

            float time = TUNING.BUILDINGS.FABRICATION_TIME_SECONDS.SHORT;

            string mycoRecipeId = ComplexRecipeManager.MakeRecipeID(SludgePressConfig.ID, mycoweaveRecipeInputs, mycoweaveRecipeOutputs);

            ComplexRecipe mycoRecipe = new ComplexRecipe(mycoRecipeId, mycoweaveRecipeInputs, mycoweaveRecipeOutputs);
            mycoRecipe.time = time;
            mycoRecipe.description = MycobrickMod.STRINGS.BUILDINGS.PREFABS.SLUDGEPRESS.MYCOWEAVE_RECIPE_DESC;
            mycoRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
            mycoRecipe.fabricators = new List<Tag> { TagManager.Create("SludgePress") };
        }

        public static void AddMycoweaveMattingRecipe()  
        {
            Debug.Log("[MycobrickMod] Adding MycoweaveMatting recipe...");

            float mycoweaveStrandsInput = 4f;
            float bleachInput = 5f;
            float mycoweaveMattingOutput = 1f;

            ComplexRecipe.RecipeElement[] mattingRecipeInputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(MycoweaveStrandsConfig.TAG, mycoweaveStrandsInput),
                new ComplexRecipe.RecipeElement(SimHashes.BleachStone.CreateTag(), bleachInput)
            };

            // *** Will output Matting after PPP changes
            ComplexRecipe.RecipeElement[] mattingRecipeOutputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), mycoweaveMattingOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
            };

            float time = TUNING.BUILDINGS.FABRICATION_TIME_SECONDS.MODERATE;

            string mattingRecipeId = ComplexRecipeManager.MakeRecipeID(ClothingFabricatorConfig.ID, mattingRecipeInputs, mattingRecipeOutputs);

            ComplexRecipe mattingRecipe = new ComplexRecipe(mattingRecipeId, mattingRecipeInputs, mattingRecipeOutputs);
            mattingRecipe.time = time;
            mattingRecipe.description = MycobrickMod.STRINGS.BUILDINGS.PREFABS.CLOTHINGFABRICATOR.MATTING_RECIPE_DESC;
            mattingRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
            mattingRecipe.fabricators = new List<Tag> { TagManager.Create("ClothingFabricator") };
        }

    public static void AddMycoweaveLungsuitRecipe()
    {
        Debug.Log("[MycobrickMod] Adding MycoweaveLungsuit recipe...");

        float mycoweaveInput = 20f;
        float lungsuitOutput = 1f;

        ComplexRecipe.RecipeElement[] lungsuitRecipeInputs = new ComplexRecipe.RecipeElement[]
        {
            new ComplexRecipe.RecipeElement("MycoweaveStrands".ToTag(), mycoweaveInput),
        };

        ComplexRecipe.RecipeElement[] lungsuitRecipeOutputs = new ComplexRecipe.RecipeElement[]
        {
                new ComplexRecipe.RecipeElement(TagManager.Create(MycoweaveLungsuitConfig.ID), lungsuitOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
        };

        float time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME;

        string lungsuitRecipeId = ComplexRecipeManager.MakeRecipeID(ClothingFabricatorConfig.ID, lungsuitRecipeInputs, lungsuitRecipeOutputs);

        ComplexRecipe lungsuitRecipe = new ComplexRecipe(lungsuitRecipeId, lungsuitRecipeInputs, lungsuitRecipeOutputs);
        lungsuitRecipe.time = time;
        lungsuitRecipe.description = MycobrickMod.STRINGS.EQUIPMENT.PREFABS.MYCOWEAVE_LUNGSUIT.RECIPE_DESC;
        lungsuitRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
        lungsuitRecipe.fabricators = new List<Tag> { TagManager.Create("ClothingFabricator") };
    }

    // public static void AddMycoweaveWarmVestRecipe()
    // {
    //     ComplexRecipe.RecipeElement[] input = new ComplexRecipe.RecipeElement[]
    //     {
    //         new ComplexRecipe.RecipeElement("MycoweaveMatting".ToTag(), (float)TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS)
    //     };
    //     ComplexRecipe.RecipeElement[] output = new ComplexRecipe.RecipeElement[]
    //     {
    //         new ComplexRecipe.RecipeElement("Warm_Vest".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
    //     };
    //     WarmVestConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingFabricator", input, output), input, output)
    //     {
    //         time = TUNING.EQUIPMENT.VESTS.WARM_VEST_FABTIME,
    //         description = global::STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC,
    //         nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
    //         fabricators = new List<Tag>
    //         {
    //             "ClothingFabricator"
    //         },
    //         sortOrder = 1
    //     };
    // }
}