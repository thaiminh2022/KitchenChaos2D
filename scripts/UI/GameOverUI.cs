using Godot;
using System;

public partial class GameOverUI : Control
{
	[Export] Label recipeDeliveredAmountText;
	[Export] Label moneyEarnedAmountText;
	[Export] Label moneySpendAmountText;

	[Export] Button mainMenuBtn;

	public override void _Ready() {
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

		mainMenuBtn.Pressed += () => {
			SceneManager.SwitchScene(SceneManager.Scene.MainMenu);
		};
			

	}


	public override void _ExitTree() {
		GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;

	}

	private void GameManager_OnStateChanged(object sender, EventArgs e) {
		if (GameManager.Instance.IsGameOver()) {
			int  successfuldeliveriesAmount = DeliveryManager.Instance.GetSuccessFulRecipeDeliveredAmount();
			
			recipeDeliveredAmountText.Text = successfuldeliveriesAmount.ToString();
			moneyEarnedAmountText.Text = MoneyManager.Instance.GetMoneyEarned().ToString();
			moneySpendAmountText.Text = MoneyManager.Instance.GetMoneySpent().ToString();

			ShowUI();
		} else {
			Hide();
		}
	}

	private void ShowUI() {
		mainMenuBtn.GrabFocus();
		Show();
	}

}
