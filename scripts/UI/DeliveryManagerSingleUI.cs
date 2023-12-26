using Godot;
using System;

public partial class DeliveryManagerSingleUI : Control
{
	[Export] Label recipeNameText;
	[Export] Control iconContainer;
	[Export] TextureRect iconTemplate;

	public override void _Ready() {
		iconTemplate.Hide();
	}

	public void SetRecipeRes(RecipeRes recipeRes) {
		recipeNameText.Text = recipeRes.recipeName;

		foreach (var child in iconContainer.GetChildren()) {
			if(child == iconTemplate) {
				continue;
			}
			
			child.QueueFree();
		}

		foreach (var kitchenObjectRes in recipeRes.kitchenObjectResList) {
			var newIconTemplate = iconTemplate.Duplicate() as TextureRect;

			newIconTemplate.Texture = kitchenObjectRes.icon;
			newIconTemplate.Show();

			iconContainer.AddChild(newIconTemplate);
		}
	}
}
