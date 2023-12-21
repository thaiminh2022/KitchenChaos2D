using Godot;
using static Godot.WebRtcDataChannel;
public partial class StoveCounter : BaseCounter
{
	public enum State {
		Idle, 
		Frying,
		Fried, 
		Burned,
	}

	// State for other components to use.
	private State state;

	[Signal] public delegate void StateChangedEventHandler(State newState);


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

	private void OnFried()
	{
		GetKitchenObject().DestroySelf();

		KitchenObject.SpawnKitchenObject(fryingRecipeRes.output, this);
		ChangeState(State.Fried);


		// Item is fried but still on the stove so it's BURNING
		burningTimer.WaitTime = burningRecipeRes.burningTimeMax;
		burningTimer.Start();

	}

	private void OnBurned()
	{
		ChangeState(State.Burned);


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
				// Player carrying something - TODO:swap
				return;
			}

			// Player is not carying anything, give kitchen object to player
			GetKitchenObject().SetKitchenObjectParent(player);

			// Reset all timers since the player picked the item up
			fryingTimer.Stop();
			burningTimer.Stop();

			ChangeState(State.Idle);
		}
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
		GD.Print("Stove state: ", state);

		// Cast to an int since variant does not allow enum
		EmitSignal(SignalName.StateChanged, (int)newState);
	}
}
