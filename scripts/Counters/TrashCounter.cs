using Godot;


public partial class TrashCounter : BaseCounter
{
	public override void Interact(Player player)
	{
		if(player.HasKitchenObject())
		{
			player.GetKitchenObject().DestroySelf();
		}
	}
}
