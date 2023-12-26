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
			DeliveryManager.Instance.DeliverRecipe(plate);

			player.GetKitchenObject().DestroySelf();
		}
	}
}
