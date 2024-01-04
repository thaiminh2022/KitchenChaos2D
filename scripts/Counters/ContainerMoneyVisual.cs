using Godot;
using System;

public partial class ContainerMoneyVisual : Control
{

	[Export] private ContainerCounter clearCounter;
	[Export] private Label moneyText;

	public override void _Ready() {
		moneyText.Text = clearCounter.GetKitchenObjectRes().itemCost.ToString();
		Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
	}

	private void Player_OnSelectedCounterChanged(object sender, BaseCounter selectedCounter) {
		if(selectedCounter == clearCounter) {
			Show();
		}else {
			Hide();
		}
	}
}
