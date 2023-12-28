using Godot;
using System;

public partial class PauseMenuUI : Control {

	[Export] private Button resumeBtn;
	[Export] private Button mainMenuBtn;

	public override void _Ready() {
		GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

		resumeBtn.Pressed += () => {
			GameManager.Instance.TogglePausing();
		};

		mainMenuBtn.Pressed += () => {
			SceneManager.SwitchScene(SceneManager.Scene.MainMenu);
		};

		Hide();
	}

	private void GameManager_OnGameResumed(object sender, EventArgs e) {
		Hide();
	}

	private void GameManager_OnGamePaused(object sender, EventArgs e) {
		Show();
	}

	public override void _ExitTree() {
		GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
		GameManager.Instance.OnGameResumed -= GameManager_OnGameResumed;

	}
}
