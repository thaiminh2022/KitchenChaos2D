using Godot;
using System;
using System.Diagnostics;

public partial class SoundManager : Node
{
	[Export] AudioClipRefRes audioClipRefRes;
	[Export] AudioStreamPlayer2D template;


	public override void _Ready() {
		DeliveryManager.Instance.OnRecipeSucceeded += DeliveryManager_OnRecipeSucceeded;
		DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
		Player.Instance.OnPlayerPickUpKitchenObject += Player_OnPlayerPickUpKitchenObject;
		
		CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
		BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
		TrashCounter.OnAnyTrashed += TrashCounter_OnAnyObjectPlaced;
		PlayerSound.OnPlayerMoved += PlayerSound_OnPlayerMoved;
	}

	private void PlayerSound_OnPlayerMoved(object sender, EventArgs e) {
		var playerSound = sender as PlayerSound;

		float boostVolumeDB = 0f;
		PlaySound(audioClipRefRes.footsteps, playerSound.GlobalPosition, boostVolumeDB);
	}

	private void TrashCounter_OnAnyObjectPlaced(object sender, EventArgs e) {
		var trashCounter = sender as TrashCounter;
		 PlaySound(audioClipRefRes.trash, trashCounter.GlobalPosition);
	}

	// static event only
	private void BaseCounter_OnAnyObjectPlaced(object sender, EventArgs e) {
		var clearCounter = sender as BaseCounter;
		PlaySound(audioClipRefRes.objectDrop, clearCounter.GlobalPosition);
	}

	private void CuttingCounter_OnAnyCut(object sender, EventArgs e) {
		var cuttingCounter = sender as CuttingCounter;
		PlaySound(audioClipRefRes.chop, cuttingCounter.GlobalPosition);
	}

	// Singletons
	private void Player_OnPlayerPickUpKitchenObject(object sender, EventArgs e) {
		PlaySound(audioClipRefRes.objectPickup, Player.Instance.GlobalPosition);
	}

	private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
		PlaySound(audioClipRefRes.deliveryFailed, DeliveryCounter.Instance.GlobalPosition);

	}

	private void DeliveryManager_OnRecipeSucceeded(object sender, EventArgs e) {
		PlaySound(audioClipRefRes.deliverySucceeded, DeliveryCounter.Instance.GlobalPosition);

	}

	private void PlaySound(AudioStream stream, Vector2 position, float boostVolumeDB = 0) {
		var streamPlayer = template.Duplicate() as AudioStreamPlayer2D;

		streamPlayer.Stream = stream;
		streamPlayer.GlobalPosition = position;
		streamPlayer.VolumeDb = boostVolumeDB;


		streamPlayer.Finished += () => streamPlayer.QueueFree();

		this.AddChild(streamPlayer);
		streamPlayer.Play();
	}

	private void PlaySound(AudioStream[] streamArr, Vector2 position, float boostVolumeDB = 0f) {
		var stream = streamArr[GD.RandRange(0, streamArr.Length - 1)];

		PlaySound(stream, position, boostVolumeDB);
	}

}
