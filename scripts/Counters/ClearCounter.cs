using Godot;


public sealed partial class ClearCounter : BaseCounter
{
	[Export] private KitchenObjectRes kitchenObjectRes;

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			// There is no kitchen object here
			if (player.HasKitchenObject())
			{
				// drop the player kitchen object in 
				player.GetKitchenObject().SetKitchenObjectParent(this);
			}
		}
		else
		{
			if (player.HasKitchenObject())
			{
				if(player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
				{
					// Player is holding a plate
					plateKitchenObject.AddIngredient(GetKitchenObject().GetKitchenObjectRes());
					GetKitchenObject().DestroySelf();

				}
			}
			else
			{
				// Player is not carying anything, give to player
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}


}
