using Godot;

public partial class ContainerMoneyVisual : Control
{

	[Export] private ContainerCounter clearCounter;
	[Export] private Label moneyText;

	[ExportGroup("Money Warning Color")]
	[Export] Color warningColor;
	[Export] Color errorColor;
	[Export] Color goodColor;



	public override void _Ready() {
		moneyText.Text = clearCounter.GetKitchenObjectRes().itemCost.ToString();
		Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
	}

	private void Player_OnSelectedCounterChanged(object sender, BaseCounter selectedCounter) {
		if(selectedCounter == clearCounter) {
			moneyText.AddThemeColorOverride("font_color", GetColorForDifferences());

			Show();
		}else {
			Hide();
		}
	}

	private Color GetColorForDifferences() {
		return clearCounter.GetMoneyDifferences() switch {
			> 0 => goodColor,
			< 0 => errorColor,
			_ => warningColor,
		};

	}
}
