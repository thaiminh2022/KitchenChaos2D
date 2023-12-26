using Godot;
using System;


public abstract partial class BaseCounter : StaticBody2D, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaced;
         

    [Export] private Marker2D counterTopPoint;

    private KitchenObject kitchenObject;

    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }

    public Marker2D GetHoldingPoint() => counterTopPoint;
    public KitchenObject GetKitchenObject() => kitchenObject;

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;

        if(newKitchenObject is not null ) {
            OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject() => kitchenObject is not null;
}