using Godot;
using System;

public partial class MainMenuUI : Control
{
	[Export] Button playButton;
	[Export] Button quitButton;

	public override void _Ready() {
		GetTree().Paused = false;
		Engine.TimeScale = 1;


		playButton.Pressed += () => {
			SceneManager.SwitchScene(SceneManager.Scene.Game);
		};
		quitButton.Pressed += () => {
			QuitGame();
		};
	}

	public override void _Process(double delta) {
	}

	private void QuitGame() {
		// Send quit event
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
	}
	
}
