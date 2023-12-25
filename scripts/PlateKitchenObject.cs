using Godot;
using System;
using System.Collections.Generic;


public partial class PlateKitchenObject : KitchenObject {

	public event EventHandler<KitchenObjectRes> OnIngredientAdded;

	private List<KitchenObjectRes> kitchenObjectResList = new();

	[Export] private KitchenObjectRes[] validIngredients;



	public bool TryAddIngredient(KitchenObjectRes kitchenObjectRes) {

		if (!IsValidIngredient(kitchenObjectRes)) {
			return false;
		}

		if (kitchenObjectResList.Contains(kitchenObjectRes)) {
			return false;
		}

		GD.Print(kitchenObjectResList.Count);

		kitchenObjectResList.Add(kitchenObjectRes);
		OnIngredientAdded?.Invoke(this, kitchenObjectRes);
		return true;
	}

	private bool IsValidIngredient(KitchenObjectRes checkIngredient) {
		foreach (var ingredient in validIngredients) {
			if (checkIngredient == ingredient) {
				return true;
			}

		}
		return false;
	}

	public List<KitchenObjectRes> GetKitchenObjectResList() {
		return kitchenObjectResList;
	}
}
