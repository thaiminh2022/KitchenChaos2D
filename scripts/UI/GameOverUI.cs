using Godot;
using System;

public partial class GameOverUI : Control
{
	[Export] Label recipeDeliveredAmountText;
    [Export] Label moneyEarnedAmountText;
	[Export] Label moneySpendAmountText;

	[Export] Button restartBtn;

    public override void _Ready() {
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        restartBtn.Pressed += () => { }
			
		;

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
		restartBtn.GrabFocus();
        Show();
    }

}
