using Godot;


public sealed partial class ContainerCounter : BaseCounter, IKitchenObjectParent
{
	[Export] private KitchenObjectRes kitchenObjectRes;


	[Signal] public delegate void PlayerGrabbedObjectEventHandler();


	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject())
		{
			// Player is carrying nothing
			KitchenObject.SpawnKitchenObject(kitchenObjectRes, player);

			EmitSignal(SignalName.PlayerGrabbedObject);
		}

	}
}

