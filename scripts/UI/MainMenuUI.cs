using Godot;
using System;

public partial class MainMenuUI : Control
{
	[Export] Button playButton;
	[Export] Button quitButton;

	public override void _Ready() {

		playButton.Pressed += () => {
			SceneManager.Instance.SwitchSceneInteractive(1);
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
