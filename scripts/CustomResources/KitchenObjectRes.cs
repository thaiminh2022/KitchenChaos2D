using Godot;
using System;

[GlobalClass]
public partial class KitchenObjectRes : Resource
{
	[Export] public PackedScene node;
	[Export] public Texture2D icon;

	[Export] public string objectName;

	[Export] public int itemCost;
}
