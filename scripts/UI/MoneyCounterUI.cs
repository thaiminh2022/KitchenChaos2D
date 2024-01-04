using Godot;
using System;

public partial class MoneyCounterUI : Control
{
	[Export] Label moneyText;

	public override void _Ready() {
		UpdateVisual();
		MoneyManager.Instance.OnMoneyChange += Instance_OnMoneyChange;
}

	private void Instance_OnMoneyChange(object sender, EventArgs e) {
		UpdateVisual();
	}

	void UpdateVisual() {
		moneyText.Text = MoneyManager.Instance.GetMoneyAmount().ToString();
	}
}
