using Godot;
using System;
using System.Collections.Generic;


public partial class PlateKitchenObject : KitchenObject {

	public event EventHandler<KitchenObjectRes> OnIngredientAdded;

	private List<KitchenObjectRes> kitchenObjectResList = new();

	[Export] private KitchenObjectRes[] validIngredients;



	public bool TryAddIngredient(KitchenObjectRes kitchenObjectRes) {

		if (!IsValidIngredient(kitchenObjectRes)) {
			GD.Print("Not valid ingredient");
			return false;
		}

		if (kitchenObjectResList.Contains(kitchenObjectRes)) {
			GD.Print("Not unique ingredient");
			return false;

		}

		GD.Print(kitchenObjectResList.Count);

		if (kitchenObjectResList.Count > 0) {
			GD.Print(kitchenObjectResList[^1] == kitchenObjectRes);
			GD.Print(kitchenObjectResList.Contains(kitchenObjectRes));
		}

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
