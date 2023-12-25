using Godot;
using System;


public partial class CuttingCounter : BaseCounter, IHasProgress {
	public event EventHandler<float> OnProgressChanged;
	public event EventHandler OnCut;


	[Export] private CuttingRecipeRes[] cuttingRecipes;

	private int cuttingProgress;

	public override void Interact(Player player) {
		if (!HasKitchenObject()) {
			// There is no kitchen object here
			if (player.HasKitchenObject()) {
				// Player holding something kitchen object
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectRes())) {
					// Drop if object has a recipe.
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;

					OnProgressChanged?.Invoke(this, 0f);

				}
			}
			else {
				// Player not carrying anything
			}
		}
		else {
			if (player.HasKitchenObject()) {
				if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) {
					// Player is holding a plate
					bool success = plateKitchenObject
						.TryAddIngredient(GetKitchenObject().GetKitchenObjectRes());

					if (success) {
						GetKitchenObject().DestroySelf();
					}

				}
			}
			else {
				// Player is not carying anything, give to player
				GetKitchenObject().SetKitchenObjectParent(player);
				OnProgressChanged?.Invoke(this, 0f);

			}
		}
	}

	public override void InteractAlternate(Player player) {
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectRes())) {
			// Start cutting the kitchen object
			cuttingProgress++;
			var recipe = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectRes());

			OnProgressChanged?.Invoke(this, (float)cuttingProgress / recipe.cuttingProgressMax);

			OnCut?.Invoke(this, EventArgs.Empty);

			if (cuttingProgress >= recipe.cuttingProgressMax) {
				// Enough cut
				GetKitchenObject().DestroySelf();
				KitchenObject.SpawnKitchenObject(recipe.output, this);

			}
		}
	}

	private bool HasRecipeWithInput(KitchenObjectRes inputKitchenObjectRes) {
		var recipe = GetCuttingRecipeWithInput(inputKitchenObjectRes);
		return recipe is not null;
	}

	private KitchenObjectRes GetOuputForInput(KitchenObjectRes inputKitchenObjectRes) {
		return GetCuttingRecipeWithInput(inputKitchenObjectRes).output;
	}

	private CuttingRecipeRes GetCuttingRecipeWithInput(KitchenObjectRes inputKitchenObjectRes) {
		foreach (var recipe in cuttingRecipes) {
			if (recipe.input == inputKitchenObjectRes) {
				return recipe;
			}
		}

		return null;
	}

}
