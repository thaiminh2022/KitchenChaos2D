using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class DeliveryManager : Node {

	public static DeliveryManager Instance { get; private set; }


	[Export] private int waitingRecipeMax = 4;

	[Export] private RecipeListRes recipeListRes;
	[Export] private Timer spawnRecipeTimer;

	private List<RecipeRes> waitingRecipeList;

	public override void _EnterTree() {
		Instance = this;
		waitingRecipeList = new();
	}

	public override void _Ready() {
		spawnRecipeTimer.Timeout += SpawnRecipe;
	}

	private void SpawnRecipe() {

		var recipeResList = recipeListRes.recipeResList;

		if (waitingRecipeList.Count < waitingRecipeMax) {

			int randomIndex = GD.RandRange(0, recipeResList.Length - 1);
			RecipeRes recipe = recipeResList[randomIndex];
			waitingRecipeList.Add(recipe);

			GD.Print("Added recipe: ", recipe.recipeName);
		}

	}
	public void DeliverRecipe(PlateKitchenObject plate) {

		RecipeRes matchedRecipe = null;

		// Checking if we have the same recipe
		foreach (var recipeRes in recipeListRes.recipeResList) {
			var plateKitchenObjectResList = plate.GetKitchenObjectResList();
			var recipeKitchenObjectResList = recipeRes.kitchenObjectResList;


			if (recipeKitchenObjectResList.Length != plateKitchenObjectResList.Count) {
				continue;
			}

			// Get the content that both exist in the plate list and the recipe list
			var intersection = plateKitchenObjectResList.Intersect(recipeKitchenObjectResList);

			// If the count of the intersection is the same as either its original parent: 
			// plate content and recipe content is the same
			if (intersection.Count() == plateKitchenObjectResList.Count) {
				// This recipe is a match
				matchedRecipe = recipeRes;
				break;
			}
		}

		if (matchedRecipe is not null) {
			// Player delivered a correct recipe
			GD.Print("Matched recipe: ", matchedRecipe.recipeName);
			waitingRecipeList.Remove(matchedRecipe);
		} else {
			// Player did not deliver a correct recipe
			GD.Print("Did not find any matched recipe");

		}

	}

}
