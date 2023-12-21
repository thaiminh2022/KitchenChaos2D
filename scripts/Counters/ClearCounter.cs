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
			else
			{
				// Player not carrying anything
			}
		}
		else
		{
			if (player.HasKitchenObject())
			{
				// Player carrying something - swap
				// Try implement swap or something i'm too lazy
			}
			else
			{
				// Player is not carying anything, give to player
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}


}
