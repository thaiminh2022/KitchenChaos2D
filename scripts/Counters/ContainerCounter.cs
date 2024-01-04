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
			// Player is carrying nothing
			KitchenObject.SpawnKitchenObject(kitchenObjectRes, player);
			MoneyManager.Instance.ReduceMoney(kitchenObjectRes.itemCost);
			
			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
		}

	}

	public KitchenObjectRes GetKitchenObjectRes() => kitchenObjectRes;
	
}

