using Godot;
using System;

public partial class DeliveryCounter : BaseCounter {

	public static DeliveryCounter Instance;

	public override void _EnterTree() {
		Instance = this;
	}

	public override void Interact(Player player) {
		
		if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out var plate)) {
			// If the player is holding any kitchen object and has a plate
			bool deliverySuccess = DeliveryManager.Instance.DeliverRecipe(plate, out var inputRecipe);

			if(deliverySuccess) {
				MoneyManager.Instance.AddMoney(inputRecipe.recipeWorth);
			}

			player.GetKitchenObject().DestroySelf();
		}
	}
}
