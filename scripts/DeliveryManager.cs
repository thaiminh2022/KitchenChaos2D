using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DeliveryManager : Node {

	public event EventHandler OnRecipeSpawn;
	public event EventHandler OnRecipeComplete;
	public event EventHandler OnRecipeSucceeded;
	public event EventHandler OnRecipeFailed;

	public static DeliveryManager Instance { get; private set; }

	public static void ResetStatic() {
		Instance = null;
	}

	[Export] private int waitingRecipeMax = 4;

	[Export] private RecipeListRes recipeListRes;
	[Export] private Timer spawnRecipeTimer;

	private List<RecipeRes> waitingRecipeList;
	private int successfulRecipeDeliveredAmount = 0;

	public override void _EnterTree() {
		Instance = this;
		waitingRecipeList = new();
	}

	public override void _Ready() {
		spawnRecipeTimer.Timeout += SpawnRecipe;
	}

	private void SpawnRecipe() {
		if(!GameManager.Instance.IsGamePlaying()) {
			return;
		}


		var recipeResList = recipeListRes.recipeResList;

		if (waitingRecipeList.Count < waitingRecipeMax) {

			int randomIndex = GD.RandRange(0, recipeResList.Length - 1);
			RecipeRes recipe = recipeResList[randomIndex];
			waitingRecipeList.Add(recipe);

			OnRecipeSpawn?.Invoke(this, EventArgs.Empty);

		}

	}
	public void DeliverRecipe(PlateKitchenObject plate) {

		RecipeRes matchedRecipe = null;

		// Checking if we have the same recipe
		foreach (var recipeRes in waitingRecipeList) {
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
			waitingRecipeList.Remove(matchedRecipe);
			successfulRecipeDeliveredAmount++;

			OnRecipeComplete?.Invoke(this, EventArgs.Empty);
			OnRecipeSucceeded?.Invoke(this, EventArgs.Empty);
		} else {
			// Player did not deliver a correct recipe
			OnRecipeFailed?.Invoke(this, EventArgs.Empty);

		}

	}

	public List<RecipeRes> GetWaitingRecipeResList() {
		return waitingRecipeList;
	}
	public int GetSuccessFulRecipeDeliveredAmount() => successfulRecipeDeliveredAmount;

}
