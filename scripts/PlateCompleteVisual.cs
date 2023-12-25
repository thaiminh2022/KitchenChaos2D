using Godot;


public partial class PlateCompleteVisual : Node2D {

	[Export] private PlateKitchenObject plateKitchenObject;
	[Export] private KitchenObjectRes_Node[] kitchenObjectRes_Nodes;

	public override void _Ready() {
		plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

		foreach (var kitchenObjectResNode in kitchenObjectRes_Nodes) {
			kitchenObjectResNode.node.Hide();
		}
	}

	private void PlateKitchenObject_OnIngredientAdded(object sender, KitchenObjectRes e) {
		foreach (var kitchenObjectResNode in kitchenObjectRes_Nodes) {
			if (e == kitchenObjectResNode.kitchenObjectRes) {
				kitchenObjectResNode.node.Show();
			}
		}
	}

}
