using Godot;

[GlobalClass]
public partial class BurningRecipeRes : Resource
{
	[Export] public KitchenObjectRes input;
	[Export] public KitchenObjectRes output;

	[Export] public float burningTimeMax;
}
