using Godot;


[GlobalClass]
public partial class FryingRecipeRes : Resource
{
	[Export] public KitchenObjectRes input;
	[Export] public KitchenObjectRes output;

	[Export] public float fryingTimerMax;

}
