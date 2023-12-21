using System.Linq.Expressions;
using Godot;

[GlobalClass]
public partial class KitchenObject : Node2D
{
	[Export] private string kitchenObjectResPath;
	private KitchenObjectRes cacheKitchenObject;

private IKitchenObjectParent kitchenObjectParent;

	public KitchenObjectRes GetKitchenObjectRes()
	{
		cacheKitchenObject ??= ResourceLoader.Load<KitchenObjectRes>(kitchenObjectResPath);

		return cacheKitchenObject;
	}

	public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
	{
		if (kitchenObjectParent is not null)
		{
			kitchenObjectParent.ClearKitchenObject();
		}

		kitchenObjectParent = newKitchenObjectParent;

		if (newKitchenObjectParent.HasKitchenObject())
		{
			string name = ((Node)newKitchenObjectParent).Name;
			GD.PrintErr(name, " already has a kitchen object");
			return;
		}
		newKitchenObjectParent.SetKitchenObject(this);

		var newParent = newKitchenObjectParent.GetHoldingPoint();
		var holder = GetParent() as Node2D;
		holder.SetParent(newParent);
		holder.Position = Vector2.Zero;
	}

	public IKitchenObjectParent GetKitchenObjectParent()
	{
		return kitchenObjectParent;
	}

	public void DestroySelf()
	{
		kitchenObjectParent.ClearKitchenObject();

		var holder = GetParent();
		holder.QueueFree();
	}

	public static KitchenObject SpawnKitchenObject(KitchenObjectRes kitchenObjectRes, IKitchenObjectParent kitchenObjectParent)
	{
		var instance = kitchenObjectRes.node.Instantiate();
		KitchenObject kitchenObject = instance.GetNode<KitchenObject>("KitchenObject");
		
		kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

		return kitchenObject;
	}
}
