using Godot;
using System.Collections.Generic;

public partial class PlateKitchenObject : KitchenObject
{

    private List<KitchenObjectRes> kitchenObjectResList;

    public override void _EnterTree()
    {
        kitchenObjectResList = new List<KitchenObjectRes>(); 
    }

    public void AddIngredient(KitchenObjectRes kitchenObjectRes)
    {
        kitchenObjectResList.Add(kitchenObjectRes);
    }
}
