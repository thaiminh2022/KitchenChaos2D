using Godot;

public partial class DeliveryManagerUI : Control {
	[Export] private PackedScene template;
	[Export] private Control container;

	public override void _Ready() {
		DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
		DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
	}

	public  override void _ExitTree() {
		DeliveryManager.Instance.OnRecipeComplete -= DeliveryManager_OnRecipeComplete;
		DeliveryManager.Instance.OnRecipeSpawn -= DeliveryManager_OnRecipeSpawn;
	}

	private void DeliveryManager_OnRecipeSpawn(object sender, System.EventArgs e) {
		UpdateVisual();
	}

	private void DeliveryManager_OnRecipeComplete(object sender, System.EventArgs e) {
		UpdateVisual();
	}

	private void UpdateVisual() {
		foreach (var child in container.GetChildren()) {
			child.QueueFree();
		}

		foreach (var waitingRecipe in DeliveryManager.Instance.GetWaitingRecipeResList()) {
			var instance = template.Instantiate<DeliveryManagerSingleUI>();

			instance.SetRecipeRes(waitingRecipe);
			container.AddChild(instance);
		}


	}



}
