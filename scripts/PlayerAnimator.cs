using Godot;
using System;
public partial class PlayerAnimator : AnimatedSprite2D
{
	[Export] private Player player;
	const string IDLE_ANIMATION = "Idle";
	const string WALK_ANIMATION = "walk";


	private AnimatedSprite2D animator;
	public override void _Ready()
	{
		// Do this for better clearance
		animator = this;

		animator.Play(IDLE_ANIMATION);
	}

	public override void _Process(double delta)
	{
		FlipVisual(player.GetLastMoveDir());


		if (player.IsWalking())
		{
			animator.Play(WALK_ANIMATION);
		}
		else
		{
			animator.Play(IDLE_ANIMATION);
		}
	}

	private void FlipVisual(Vector2 direction)
	{
		if (direction.X > 0)
		{
			Rotation = Mathf.Pi;
			Scale = new(1, -1);
		}
		else
		{
			Rotation = 0;
			Scale = new(1, 1);
		}
	}
}
