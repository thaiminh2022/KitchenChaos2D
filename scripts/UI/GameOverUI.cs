using Godot;
using System;

public partial class GameOverUI : Control
{
	[Export] Label recipeDeliveredAmountText;

	public override void _Ready() {
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
	}

	public override void _ExitTree() {
		GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;

	}

	private void GameManager_OnStateChanged(object sender, EventArgs e) {
		if (GameManager.Instance.IsGameOver()) {
			int  successfuldeliveriesAmount = DeliveryManager.Instance.GetSuccessFulRecipeDeliveredAmount();
			recipeDeliveredAmountText.Text = successfuldeliveriesAmount.ToString();

			Show();
		} else {
			Hide();
		}
	}


}
