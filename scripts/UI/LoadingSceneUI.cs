using Godot;
using System;
using System.Threading.Tasks;

public partial class LoadingSceneUI : Control
{
	private bool animationFinished = false;
	public override async void _Ready() {
		await ArtificialWait();
	}

	private async Task ArtificialWait() {
		await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
		animationFinished = true;
	}

	public bool IsAnimationFinished() {
		return animationFinished;
	}

	
}
