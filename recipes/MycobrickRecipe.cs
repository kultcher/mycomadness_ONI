using System.Collections.Generic; // For List in RecipeElement
using UnityEngine;
//using MycobrickMod.Utils; // For ModStrings, if used for recipe description
//using TUNING;
using MycobrickMod.Elements;
// using static ComplexRecipe; // If you want to use RecipeElement directly without ComplexRecipe.RecipeElement
using MycobrickMod.Utils;
// It's good practice to ensure TUNING values are available if you use them for recipe times/amounts.
// For example, by having a using static TUNING.BUILDINGS.FABRICATION_TIME;

namespace MycobrickMod.Recipes
{
    public static class MycobrickRecipe
    {
        public const string MycofiberToMycobricksRecipeId = "MycofiberToMycobricks";

        public static void AddMycobricksRecipe()
        {
            // Define the input: 50kg Mycofiber
            ComplexRecipe.RecipeElement[] inputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(MycofiberElement.ID, 50f) // Use string ID of Mycofiber
            };

            ComplexRecipe.RecipeElement[] outputs = new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(MycobrickElement.ID, 100f) // Use string ID of Mycobricks
            };

            // Recipe Time (example: medium duration)
            // float time = TUNING.BUILDINGS.FABRICATION_TIME.MEDIUM_FABRICATION_TIME; // Example: 40s
            // Using a hardcoded value for now if TUNING isn't fully available or for simplicity.
            float time = TUNING.BUILDINGS.FABRICATION_TIME_SECONDS.VERY_SHORT;

            // Description for the recipe UI
            string description = STRINGS.BUILDINGS.PREFABS.KILN.MYCOBRICK_RECIPE_DESC;

            // Create the recipe details object
            var recipe = new ComplexRecipe(
                ComplexRecipeManager.MakeRecipeID(KilnConfig.ID, inputs, outputs), // Unique recipe ID
                inputs,
                outputs
            );

            recipe.requiredTech = null;

            recipe.time = time;
            recipe.description = description;
            recipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result; // Shows "Result: Mycobricks"
            recipe.fabricators = new List<Tag> { new Tag(KilnConfig.ID) }; // Specify Kiln as the fabricator
                                                                           // recipe.requiredTech = null; // Set if a specific tech is needed, e.g., "BasicRefinement"

            // Add the recipe to the game's recipe manager
            // This is a common way, but sometimes recipes are added directly to the building's list.

            //ComplexRecipeManager.Get().Add(recipe);
            Debug.Log($"MycoMod: Added recipe '{recipe.id}' - {MycobrickElement.ID} to {MycobrickElement.ID} in Kiln.");
        }
    }
}