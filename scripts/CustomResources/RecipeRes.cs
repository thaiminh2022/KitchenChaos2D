using Godot;

[GlobalClass]
public partial class RecipeRes : Resource {
    [Export] public KitchenObjectRes[] kitchenObjectResList;
    [Export] public string recipeName;
    [Export] public int recipeWorth;
}
