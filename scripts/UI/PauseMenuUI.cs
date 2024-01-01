using Godot;
using System;

public partial class PauseMenuUI : Control {

	public static PauseMenuUI Instance { get; private set; }
	public static void ResetStatic() {
		Instance = null;
	}

	[Export] private Button resumeBtn;
	[Export] private Button settingsBtn;
	[Export] private Button mainMenuBtn;


	public override void _EnterTree() {
		Instance = this;
	}

	public override void _Ready() {
		GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

		resumeBtn.Pressed += () => {
			GameManager.Instance.TogglePausing();
		};

		mainMenuBtn.Pressed += () => {
			SceneManager.SwitchScene(SceneManager.Scene.MainMenu);
		};

		settingsBtn.Pressed += () => {
			SettingsUI.Instance.ShowUI();
			Hide();

		};

		Hide();
	}

	private void GameManager_OnGameResumed(object sender, EventArgs e) {
		Hide();
	}

	private void GameManager_OnGamePaused(object sender, EventArgs e) {
		ShowUI();
	}
	
	public void ShowUI() {
		Show();
		resumeBtn.GrabFocus();
	}
}
