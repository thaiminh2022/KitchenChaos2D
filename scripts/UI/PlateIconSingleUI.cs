using Godot;
using System;

public partial class PlateIconSingleUI : Control
{
	[Export]
	private TextureRect image;

	public void SetKitchenObjectRes(KitchenObjectRes kitchenObjectRes) {
		image.Texture = kitchenObjectRes.icon;
	}
}
