using Godot;
using System;

public partial class MainMenuUI : Control
{
	[Export] Button playButton;
	[Export] Button quitButton;

	public override void _Ready() {

		playButton.Pressed += () => {
			SceneManager.SwitchSceneInteractive(SceneManager.Scene.MainGame);
		};
		quitButton.Pressed += () => {
			QuitGame();
		};
	}

	private void QuitGame() {
		// Send quit event
		GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
	}
	
}
