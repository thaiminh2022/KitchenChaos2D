using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class StoveCounterVisual : AnimatedSprite2D
{
	private const string FRYING = "frying";
	private const string IDLE = "idle";


	[Export] private StoveCounter stoveCoutner;
	[Export] private CpuParticles2D fryingParticles;
	[Export] private PointLight2D pointLight;

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
			pointLight.Show();
		}else
		{
			Play(IDLE);
			fryingParticles.Hide();
			pointLight.Hide();
			
		}
	}

	public override void _ExitTree() {
		stoveCoutner.OnStateChanged -= StoveCoutner_StateChanged;

	}
}
