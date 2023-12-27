using Godot;
using System;

public partial class GameplayClockUI : Control
{
	[Export] TextureProgressBar clockBar;

	public override void _Process(double delta) {
		clockBar.Value = GameManager.Instance.GetGameplayTimerNormalized();
	}
}
