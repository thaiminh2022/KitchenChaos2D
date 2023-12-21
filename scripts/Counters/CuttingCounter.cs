using Godot;


public partial class CuttingCounter : BaseCounter, IHasProgress
{
	[Signal]
	public delegate void ProgressChangedEventHandler(float progressNormalized);
	[Signal]
	public delegate void CutEventHandler();


	[Export] private CuttingRecipeRes[] cuttingRecipes;

	private int cuttingProgress;

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			// There is no kitchen object here
			if (player.HasKitchenObject())
			{
				// Player holding something kitchen object
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectRes()))
				{
					// Drop if object has a recipe.
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;

					EmitSignal(SignalName.ProgressChanged, 0f);

				}
			}
			else
			{
				// Player not carrying anything
			}
		}
		else
		{
			if (player.HasKitchenObject())
			{
				// Player carrying something - swap
			}
			else
			{
				// Player is not carying anything, give to player
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}

	public override void InteractAlternate(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectRes()))
		{
			// Start cutting the kitchen object
			cuttingProgress++;
			var recipe = GetCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectRes());
			
			EmitSignal(SignalName.ProgressChanged, (float)cuttingProgress / recipe.cuttingProgressMax);
			EmitSignal(SignalName.Cut);

			if (cuttingProgress >= recipe.cuttingProgressMax)
			{
				// Enough cut
				GetKitchenObject().DestroySelf();
				KitchenObject.SpawnKitchenObject(recipe.output, this);

			}
		}
	}

	private bool HasRecipeWithInput(KitchenObjectRes inputKitchenObjectRes)
	{
		var recipe = GetCuttingRecipeWithInput(inputKitchenObjectRes);
		return recipe is not null;
	}

	private KitchenObjectRes GetOuputForInput(KitchenObjectRes inputKitchenObjectRes)
	{
		return GetCuttingRecipeWithInput(inputKitchenObjectRes).output;
	}

	private CuttingRecipeRes GetCuttingRecipeWithInput(KitchenObjectRes inputKitchenObjectRes)
	{
		foreach (var recipe in cuttingRecipes)
		{
			if (recipe.input == inputKitchenObjectRes)
			{
				return recipe;
			}
		}

		return null;
	}

}
