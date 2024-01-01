using Godot;
using System;

public partial class GameStartCountDownUI : Control {
	private const string NUMBER_POPUP_ANIMATION = "number_popup";

	[Export] private Label countDownText;
	[Export] private AnimationPlayer textAnim;

	private int previousCountdownNumber;

	public override void _Ready() {
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
	}


	private void GameManager_OnStateChanged(object sender, EventArgs e) {
		if (GameManager.Instance.IsCountdownToStartActive()) {
			Show();
		} else {
			Hide();
		}
	}

	public override void _Process(double delta) {
		if (Visible) {
			int currentCountdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimeLeft());
			countDownText.Text = currentCountdownNumber.ToString();

			if (previousCountdownNumber != currentCountdownNumber) {
				if(textAnim.IsPlaying()) {
					textAnim.Stop();
				}
				textAnim.Play(NUMBER_POPUP_ANIMATION);
				SoundManager.Instance.PlayCountdownSound();
				previousCountdownNumber = currentCountdownNumber;
			}


		}
	}
}
