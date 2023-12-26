using Godot;

public partial class PlateIconUI : Control {

	[Export] private PlateKitchenObject plateKitchenObject;
	[Export] private PackedScene iconTemplate;


	public override void _Ready() {
		plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
	}

	private void PlateKitchenObject_OnIngredientAdded(object sender, KitchenObjectRes e) {
		UpdateVisual();
	}

	private void UpdateVisual() {

		foreach (var c in GetChildren()) {
			c.QueueFree();
		}

		foreach (var kitchenObjectRes in plateKitchenObject.GetKitchenObjectResList()) {
			PlateIconSingleUI instance = iconTemplate.Instantiate<PlateIconSingleUI>();
			instance.SetKitchenObjectRes(kitchenObjectRes);

			AddChild(instance);
		}
	}
}
