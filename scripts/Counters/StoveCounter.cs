using Godot;
using System;

public sealed partial class StoveCounter : BaseCounter, IHasProgress {
	public enum State {
		Idle, 
		Frying,
		Fried, 
		Burned,
	}

	// State for other components to use.
	private State state;
	public event EventHandler<State> OnStateChanged;
	public event EventHandler<float> OnProgressChanged;

	[Export] private FryingRecipeRes[] fryingRecipes;
	[Export] private BurningRecipeRes[] burningRecipes;

	[Export] Timer fryingTimer;
	[Export] Timer burningTimer;


	private FryingRecipeRes fryingRecipeRes;
	private BurningRecipeRes burningRecipeRes;


	public override void _Ready()
	{
		ChangeState(State.Idle);

		fryingTimer.Timeout += OnFried;
		burningTimer.Timeout += OnBurned;

	}

	public override void _Process(double delta)
	{
		if(!fryingTimer.IsStopped())
		{
			// Stove is still frying stuff
			OnProgressChanged?.Invoke(this, 1 - (float)fryingTimer.GetTimeProgressNormalized());

		}

		if (!burningTimer.IsStopped())
		{
			OnProgressChanged?.Invoke(this, (float)burningTimer.GetTimeProgressNormalized());
		}
	}


	private void OnFried()
	{
		GetKitchenObject().DestroySelf();
		KitchenObject.SpawnKitchenObject(fryingRecipeRes.output, this);
		
		ChangeState(State.Fried);

		fryingTimer.Stop();

		// Item is fried but still on the stove so it's BURNING
		burningTimer.WaitTime = burningRecipeRes.burningTimeMax;
		burningTimer.Start();

	}

	private void OnBurned()
	{
		ChangeState(State.Burned);

		// Invoke to clamped the value 
		OnProgressChanged?.Invoke(this, 0f);

		// Destroy the normal object
		GetKitchenObject().DestroySelf();

		// Spawn the charcoal-ed object
		KitchenObject.SpawnKitchenObject(burningRecipeRes.output, this);
	}



	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			// There is no kitchen object here
			if (!player.HasKitchenObject())
				return;

			// Player holding a kitchen object
			if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectRes()))
			{
				// Drop if object has a recipe (can be fried).
				player.GetKitchenObject().SetKitchenObjectParent(this);

				// cache all the nessesary recipes
				fryingRecipeRes =
					GetFryingRecipeWithInput(GetKitchenObject().GetKitchenObjectRes());
				burningRecipeRes =
					GetBurningRecipeWithInput(fryingRecipeRes.output);

				// Set up frying timer
				fryingTimer.WaitTime = fryingRecipeRes.fryingTimerMax;
				fryingTimer.Start();

				ChangeState(State.Frying);
			}
		}
		else
		{
			// The counter does not has any object on it

			if (player.HasKitchenObject())
			{
				if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject)) {
					// Player is holding a plate
					bool success = plateKitchenObject
						.TryAddIngredient(GetKitchenObject().GetKitchenObjectRes());

					if (success) {
						GetKitchenObject().DestroySelf();
						ResetState();
					}

				}

			}
			else {
				// Player is not carying anything, give kitchen object to player
				GetKitchenObject().SetKitchenObjectParent(player);

				ResetState();
			}

			
		}
	}

	private void ResetState() {
		// Reset all timers since the player picked the item up
		fryingTimer.Stop();
		burningTimer.Stop();

		ChangeState(State.Idle);
		OnProgressChanged?.Invoke(this, 0f);
	}

	private bool HasRecipeWithInput(KitchenObjectRes inputKitchenObjectRes)
	{
		var recipe = GetFryingRecipeWithInput(inputKitchenObjectRes);
		return recipe is not null;
	}

	private KitchenObjectRes GetOuputForInput(KitchenObjectRes inputKitchenObjectRes)
	{
		return GetFryingRecipeWithInput(inputKitchenObjectRes).output;
	}

	private FryingRecipeRes GetFryingRecipeWithInput(KitchenObjectRes inputKitchenObjectRes)
	{
		foreach (var recipe in fryingRecipes)
		{
			if (recipe.input == inputKitchenObjectRes)
			{
				return recipe;
			}
		}

		return null;
	}

	private BurningRecipeRes GetBurningRecipeWithInput(KitchenObjectRes inputKitchenObjectRes)
	{
		foreach (var recipe in burningRecipes)
		{
			if (recipe.input == inputKitchenObjectRes)
			{
				return recipe;
			}
		}

		return null;
	}


	private void ChangeState(State newState)
	{
		state = newState;
		//GD.Print("Stove state: ", state);

		// Cast to an int since variant does not allow enum
		OnStateChanged?.Invoke(this, state);
	}

	public override void _ExitTree() {
		fryingTimer.Timeout -= OnFried;
		burningTimer.Timeout -= OnBurned;
	}
}
