using Godot;
using System;

public partial class PlayerSound : Node2D
{
	public static event EventHandler OnPlayerMoved;

	public static void ResetStatic() {
		OnPlayerMoved = null;
	}

	[Export] Player player;
	[Export] Timer footStepTimer;

	public override void _Ready() {
		footStepTimer.Timeout += FootStepTimer_Timeout;
	}

	private void FootStepTimer_Timeout() {
		if(player.IsWalking()) {
			OnPlayerMoved?.Invoke(this, EventArgs.Empty);
		}
	}
}
