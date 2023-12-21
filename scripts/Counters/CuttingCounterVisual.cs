using Godot;


public partial class CuttingCounterVisual : AnimatedSprite2D
{
	private const string CUT = "cut";
	private const string IDLE = "idle";


	[Export] private CuttingCounter cuttingCounter;
	private string currentAnimation;

	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;

		cuttingCounter.Cut += CuttingCounter_OnCut;


		PlayAnimation(IDLE);
	}

	private void CuttingCounter_OnCut()
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
}
