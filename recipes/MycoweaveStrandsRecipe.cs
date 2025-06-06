using System.Collections.Generic; 
using MycobrickMod.Elements;

namespace MycobrickMod.Recipes
{
    public static class MycoweaveStrandsRecipe
    {
        public const string MycofiberToMycoweaveStrands = "MycofiberToMycoweaveStrands";

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
                new ComplexRecipe.RecipeElement(TagManager.Create(MycoweaveStrandsConfig.ID), mycoweaveOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
                new ComplexRecipe.RecipeElement(SimHashes.DirtyWater.CreateTag(), pollutedWaterOutput, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, true)            };

            float time = TUNING.BUILDINGS.FABRICATION_TIME_SECONDS.SHORT;

            string mycoRecipeId = ComplexRecipeManager.MakeRecipeID(SludgePressConfig.ID, mycoweaveRecipeInputs, mycoweaveRecipeOutputs);
            
            ComplexRecipe mycoRecipe = new ComplexRecipe(mycoRecipeId, mycoweaveRecipeInputs, mycoweaveRecipeOutputs);
            mycoRecipe.time = time;
            mycoRecipe.description = MycobrickMod.STRINGS.BUILDINGS.PREFABS.SLUDGEPRESS.MYCOWEAVE_RECIPE_DESC;
            mycoRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
            mycoRecipe.fabricators = new List<Tag> { TagManager.Create("SludgePress") };

        }
    }
}