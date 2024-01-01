using Godot;
public partial class PlayerAnimator : AnimatedSprite2D {
    
    [Export] private CpuParticles2D movingParticle;
    [Export] private Player player;
    private const string IDLE_ANIMATION = "Idle";
    private const string WALK_ANIMATION = "walk";


    private AnimatedSprite2D animator;
    public override void _Ready() {
        // Do this for better clearance
        animator = this;

        animator.Play(IDLE_ANIMATION);
    }

    public override void _Process(double delta) {
        FlipVisual(player.GetLastMoveDir());


        if (player.IsWalking()) {
            animator.Play(WALK_ANIMATION);
            movingParticle.Emitting = true;

        } else {
            animator.Play(IDLE_ANIMATION);
            movingParticle.Emitting = false;
        }
    }

    private void FlipVisual(Vector2 direction) {
        if (direction.X > 0) {
            Rotation = Mathf.Pi;
            Scale = new(1, -1);
        } else {
            Rotation = 0;
            Scale = new(1, 1);
        }
    }
}
