using Godot;
using System;


public partial class CuttingCounterVisual : AnimatedSprite2D
{
	private const string CUT = "cut";
	private const string IDLE = "idle";


	[Export] private CuttingCounter cuttingCounter;
	private string currentAnimation;

	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
		cuttingCounter.OnCut += CuttingCounter_OnCut;


		PlayAnimation(IDLE);
	}

	private void CuttingCounter_OnCut(object sender, EventArgs e)
	{
		PlayAnimation(CUT);
	}

	private void OnAnimationFinished()
	{
		if (currentAnimation == CUT)
		{
			PlayAnimation(IDLE);
		}
	}

	private void PlayAnimation(string name)
	{
		Stop();
		Play(name);
		currentAnimation = name;
	}

	public override void _ExitTree() {
		AnimationFinished -= OnAnimationFinished;
		cuttingCounter.OnCut -= CuttingCounter_OnCut;
	}
}
