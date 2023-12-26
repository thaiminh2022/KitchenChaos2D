using Godot;
using System;


public partial class TrashCounter : BaseCounter
{
	public static event EventHandler OnAnyTrashed;

	public override void Interact(Player player)
	{
		if(player.HasKitchenObject())
		{
			player.GetKitchenObject().DestroySelf();
			OnAnyTrashed?.Invoke(this, EventArgs.Empty);

		}
	}
}
