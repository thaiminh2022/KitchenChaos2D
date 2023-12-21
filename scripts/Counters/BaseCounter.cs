using Godot;


public abstract partial class BaseCounter : StaticBody2D, IKitchenObjectParent
{
    [Export] private Marker2D counterTopPoint;

    private KitchenObject kitchenObject;

    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }

    public Marker2D GetHoldingPoint() => counterTopPoint;
    public KitchenObject GetKitchenObject() => kitchenObject;

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject() => kitchenObject is not null;
}