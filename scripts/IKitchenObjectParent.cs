using Godot;

public interface IKitchenObjectParent
{
	Marker2D GetHoldingPoint();
	KitchenObject GetKitchenObject();

	void SetKitchenObject(KitchenObject newKitchenObject);
	void ClearKitchenObject();

	bool HasKitchenObject();
}
