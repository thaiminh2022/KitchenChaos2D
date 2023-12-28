using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class StoveCounterVisual : AnimatedSprite2D
{
	private const string FRYING = "frying";
	private const string IDLE = "idle";


	[Export] private StoveCounter stoveCoutner;
	[Export] private CpuParticles2D fryingParticles;

	public override void _Ready()
	{
		stoveCoutner.OnStateChanged += StoveCoutner_StateChanged;
	}

	private void StoveCoutner_StateChanged(object sender, StoveCounter.State newState)
	{
		bool showVisual = newState == StoveCounter.State.Frying 
			|| newState == StoveCounter.State.Fried;

		if(showVisual)
		{
			Play(FRYING);
			fryingParticles.Show();
		}else
		{
			Play(IDLE);
			fryingParticles.Hide();
		}
	}

	public override void _ExitTree() {
		stoveCoutner.OnStateChanged -= StoveCoutner_StateChanged;

	}
}
