using Godot;


public sealed partial class ClearCounter : BaseCounter {


	[Export] private KitchenObjectRes kitchenObjectRes;

	public override void Interact(Player player) {

		if (!HasKitchenObject()) {
			// There is no kitchen object here
			if (player.HasKitchenObject()) {
				// drop the player kitchen object in 
				player.GetKitchenObject().SetKitchenObjectParent(this);
			}
		} else {
			if (player.HasKitchenObject()) {
				if (player.GetKitchenObject().TryGetPlate(out var plate)) {
					// Player is holding a plate
					bool success = plate
						.TryAddIngredient(GetKitchenObject().GetKitchenObjectRes());

					if (success) {
						GetKitchenObject().DestroySelf();
					}

				} else {
					// Player is not holding a plate
					if(GetKitchenObject().TryGetPlate(out plate)) {
						// Counter is holding plate
						bool success = plate.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectRes());

						if(success) {
							player.GetKitchenObject().DestroySelf();
						}
					}
				}
			} else {
				// Player is not carying anything, give to player
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}


}
