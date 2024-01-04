using Godot;
using System;


public sealed partial class ContainerCounter : BaseCounter, IKitchenObjectParent
{
	[Export] private KitchenObjectRes kitchenObjectRes;

	public event EventHandler OnPlayerGrabbedObject;

	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject())
		{
			MoneyManager.Instance.ReduceMoney(kitchenObjectRes.itemCost);
			
			// Player is carrying nothing
			KitchenObject.SpawnKitchenObject(kitchenObjectRes, player);
			
			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
		}

	}
	public int GetMoneyDifferences() {
		return MoneyManager.Instance.GetMoneyAmount() - kitchenObjectRes.itemCost;

	}

	public KitchenObjectRes GetKitchenObjectRes() => kitchenObjectRes;
	
}

