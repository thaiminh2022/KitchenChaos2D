using Godot;


public partial class Player : CharacterBody2D, IKitchenObjectParent
{
	public static Player Instance { get; private set; }


	[Signal]
	public delegate void SelectedCounterChangeEventHandler(BaseCounter selectedCounter);

	[Export] private float moveSpeed;
	[Export] private RayCast2D rayCast;
	[Export] private Marker2D playerHoldingPoint;


	private bool isWalking = false;
	private BaseCounter selectedCounter;
	private Vector2 lastMoveDir;
	private float moveDistance;
	private KitchenObject kitchenObject;

	public override void _PhysicsProcess(double delta)
	{

		HandleMove();
		HandleRayCasting(delta);

		HandleInteract();
		HandleAltInteract();
	}

	private void HandleInteract()
	{
		if (Input.IsActionJustPressed(Constants.INTEREACT))
		{
			selectedCounter?.Interact(this);
		}
	}
	private void HandleAltInteract()
	{
		if (Input.IsActionJustPressed(Constants.ALT_INTEREACT))
		{
			selectedCounter?.InteractAlternate(this);
		}
	}

	private void HandleRayCasting(double delta)
	{
		float pixelMultiplier = 15;
		moveDistance = moveSpeed * (float)delta * pixelMultiplier;
		rayCast.TargetPosition = lastMoveDir * moveDistance;

		if (rayCast.IsColliding())
		{
			// Is Counter
			var collider = rayCast.GetCollider();
			if (collider is BaseCounter baseCounter)
			{
				// Intereacted with counter
				if (selectedCounter != baseCounter)
				{
					SetSelectedCounter(baseCounter);
				}
			}
			else
			{
				// Not counter
				SetSelectedCounter(null);
			}
		}
		else
		{
			SetSelectedCounter(null);
		}
	}
	private void SetSelectedCounter(BaseCounter newSelectedCounter)
	{
		selectedCounter = newSelectedCounter;
		EmitSignal(SignalName.SelectedCounterChange, selectedCounter);
	}

	private void HandleMove()
	{
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector(
			Constants.MOVE_LEFT,
			Constants.MOVE_RIGHT,
			Constants.MOVE_UP,
			Constants.MOVE_DOWN
		).Normalized();


		if (direction != Vector2.Zero)
		{
			isWalking = true;
			lastMoveDir = direction;

			velocity = direction * moveSpeed;
		}
		else
		{
			isWalking = false;
			velocity = velocity.MoveToward(Vector2.Zero, moveSpeed);
		}
		Velocity = velocity;
		MoveAndSlide();

	}

	public bool IsWalking()
	{
		return isWalking;
	}
	public Vector2 GetLastMoveDir()
	{
		return lastMoveDir;
	}

	public override void _EnterTree()
	{

		if (Instance != null)
		{
			GD.PrintErr("There's more than one player instance");
		}
		Instance = this;
	}

	public Marker2D GetHoldingPoint() => playerHoldingPoint;
	public KitchenObject GetKitchenObject() => kitchenObject;

	public void SetKitchenObject(KitchenObject newKitchenObject)
	{
		kitchenObject = newKitchenObject;
	}
	public void ClearKitchenObject()
	{
		kitchenObject = null;
	}

	public bool HasKitchenObject() => kitchenObject is not null;
}