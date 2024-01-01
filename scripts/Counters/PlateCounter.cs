using Godot;
using System;

public partial class PlateCounter : BaseCounter
{
	public event EventHandler OnPlateSpawn;
	public event EventHandler OnPlateRemoved;


	[Export] Timer SpawnPlateTimer;
	[Export] KitchenObjectRes plateObjectRes;

	private int plateSpawnAmount;
	private int plateSpawnAmountMax = 4;


	public override void _Ready()
	{
		SpawnPlateTimer.Timeout += SpawnPlate;

		// Invoke so the UI auto updated
		//OnPlateSpawn?.Invoke(this, EventArgs.Empty);
	}

	private void SpawnPlate()
	{
		if (!GameManager.Instance.IsGamePlaying()) {
			return;
		}


		if (plateSpawnAmount < plateSpawnAmountMax)
		{
			plateSpawnAmount++;
			OnPlateSpawn?.Invoke(this, EventArgs.Empty);
		}
	}

	public override void Interact(Player player) {

		if (!player.HasKitchenObject()) { 
			// Player is empty handed

			if(plateSpawnAmount > 0)
			{
				// At least one plate
				plateSpawnAmount--;
				KitchenObject.SpawnKitchenObject(plateObjectRes, player);
				OnPlateRemoved?.Invoke(this, EventArgs.Empty);
			}
		}

	}
}



