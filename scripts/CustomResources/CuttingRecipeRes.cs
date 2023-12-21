using Godot;
using System;


[GlobalClass]
public partial class CuttingRecipeRes : Resource
{
	[Export] public KitchenObjectRes input;
	[Export] public KitchenObjectRes output;

	[Export] public int cuttingProgressMax;

}
