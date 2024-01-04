using Godot;


public partial class ContainerCounterVisual : AnimatedSprite2D
{
	private const string OPEN = "open";
	private const string CLOSE = "close";
	private const string IDLE = "idle";


	[Export] private ContainerCounter containerCounter;
	private string currentAnimation;

	public override void _Ready()
	{
		containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject; ;
		AnimationFinished += OnAnimationFinished;

		PlayAnimation(IDLE);
	}

	private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
		PlayAnimation(OPEN);
	}

	private void OnAnimationFinished()
	{
		if (currentAnimation == OPEN)
		{
			PlayAnimation(CLOSE);
		}
		else if (currentAnimation == CLOSE)
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
