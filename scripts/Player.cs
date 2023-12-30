using Godot;
using System;


public partial class Player : CharacterBody2D, IKitchenObjectParent {
	public static Player Instance { get; private set; }

	public static void ResetStatic() {
		Instance = null;

	}

	public event EventHandler<BaseCounter> OnSelectedCounterChanged;
	public event EventHandler OnPlayerPickUpKitchenObject;


	[Export] private float moveSpeed;
	[Export] private RayCast2D rayCast;
	[Export] private Marker2D playerHoldingPoint;


	private bool isWalking = false;
	private BaseCounter selectedCounter;
	private Vector2 lastMoveDir;
	private float rayDistance;
	private KitchenObject kitchenObject;

	public override void _PhysicsProcess(double delta) {

		HandleMove();
		HandleRayCasting(delta);

		HandleInteract();
		HandleAltInteract();
	}

	private void HandleInteract() {

		if (Input.IsActionJustPressed(Constants.Bindings.Interact.ToString())) {
			if (!GameManager.Instance.IsGamePlaying())
				return;

			selectedCounter?.Interact(this);
		}
	}
	private void HandleAltInteract() {

		if (Input.IsActionJustPressed(Constants.Bindings.Alt_Interact.ToString())) {
			if (!GameManager.Instance.IsGamePlaying())
				return;

			selectedCounter?.InteractAlternate(this);
		}
	}

	private void HandleRayCasting(double delta) {
		float pixel_multiplier = 10f;
		rayDistance = moveSpeed * (float)delta * pixel_multiplier;
		rayCast.TargetPosition = lastMoveDir * rayDistance;

		if (rayCast.IsColliding()) {
			// Is Counter
			var collider = rayCast.GetCollider();
			if (collider is BaseCounter baseCounter) {
				// Intereacted with counter
				if (selectedCounter != baseCounter) {
					SetSelectedCounter(baseCounter);
				}
			} else {
				// Not counter
				SetSelectedCounter(null);
			}
		} else {
			SetSelectedCounter(null);
		}
	}
	private void SetSelectedCounter(BaseCounter newSelectedCounter) {
		selectedCounter = newSelectedCounter;
		OnSelectedCounterChanged?.Invoke(this, selectedCounter);
	}

	private void HandleMove() {
		Vector2 velocity = Velocity;
		Vector2 direction = Input.GetVector(
			Constants.Bindings.Move_Left.ToString(),
				Constants.Bindings.Move_Right.ToString() ,
			Constants.Bindings.Move_Up.ToString(),
			Constants.Bindings.Move_Down.ToString()
		).Normalized();


		if (direction != Vector2.Zero) {
			isWalking = true;
			lastMoveDir = direction;

			velocity = direction * moveSpeed;
		} else {
			isWalking = false;
			velocity = velocity.MoveToward(Vector2.Zero, moveSpeed);
		}
		Velocity = velocity;
		MoveAndSlide();

	}

	public bool IsWalking() {
		return isWalking;
	}
	public Vector2 GetLastMoveDir() {
		return lastMoveDir;
	}

	public override void _EnterTree() {
		Instance = this;
	}

	public Marker2D GetHoldingPoint() => playerHoldingPoint;
	public KitchenObject GetKitchenObject() => kitchenObject;

	public void SetKitchenObject(KitchenObject newKitchenObject) {
		kitchenObject = newKitchenObject;

		if (kitchenObject is not null) {
			OnPlayerPickUpKitchenObject?.Invoke(this, EventArgs.Empty);

		}

	}
	public void ClearKitchenObject() {
		kitchenObject = null;
	}

	public bool HasKitchenObject() => kitchenObject is not null;
}
