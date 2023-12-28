using Godot;
using System;

public partial class GameStartCountDownUI : Control {
	[Export] private Label countDownText;

	public override void _Ready() {
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
	}

	private void GameManager_OnStateChanged(object sender, EventArgs e) {
		if (GameManager.Instance.CountdownToStartActive()) {
			Show();
		} else {
			Hide();
		}
	}

	public override void _Process(double delta) {
		if (Visible) {
			double timeLeft = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimeLeft());
			countDownText.Text = timeLeft.ToString();
		}
	}
	public override void _ExitTree() {
		GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;

	}
}
