using Godot;
using System;

public partial class SoundManager : Node
{
	const string SOUNDMANAGER_SECTION = "volume";

	public static SoundManager Instance { get; private set; }
	public static void ResetStatic() {
		Instance = null;
	}

	[Export] AudioClipRefRes audioClipRefRes;
	[Export] AudioStreamPlayer2D template;

	public enum VolumeBus {
		Master,
		Background,
		VFX,
	}

	private ConfigFile configFile;


	public override void _EnterTree() {
		Instance = this;
		configFile = Constants.GetSettingsConfigFile();
		
		LoadVolume();

	}
	public override void _Ready() {
		LoadVolume();


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
		PlaySound(audioClipRefRes.footsteps, playerSound.GlobalPosition);
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
		var player = sender as Player;
		PlaySound(audioClipRefRes.objectPickup, player.GlobalPosition);
	}

	private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
		PlaySound(audioClipRefRes.deliveryFailed, DeliveryCounter.Instance.GlobalPosition);

	}

	private void DeliveryManager_OnRecipeSucceeded(object sender, EventArgs e) {
		PlaySound(audioClipRefRes.deliverySucceeded, DeliveryCounter.Instance.GlobalPosition);

	}
	public void PlayCountdownSound() {
		PlaySound(audioClipRefRes.warning, Vector2.Zero);
	}
	public void PlayWarningSound() {
        PlaySound(audioClipRefRes.warning, Vector2.Zero);
    }


    private void PlaySound(AudioStream stream, Vector2 position) {
		var streamPlayer = template.Duplicate() as AudioStreamPlayer2D;

		streamPlayer.Stream = stream;
		streamPlayer.GlobalPosition = position;


		streamPlayer.Finished += () => streamPlayer.QueueFree();

		this.AddChild(streamPlayer);
		streamPlayer.Play();
	}

	private void PlaySound(AudioStream[] streamArr, Vector2 position) {
		var stream = streamArr[GD.RandRange(0, streamArr.Length - 1)];

		PlaySound(stream, position);
	}


	public void ChangeVolume(VolumeBus bus, float volumeDB) {
		AudioServer.SetBusVolumeDb((int)bus, volumeDB);

		// Save the volume for user
		configFile.SetValue(SOUNDMANAGER_SECTION, bus.ToString(), volumeDB);
		configFile.Save(Constants.GAME_SETTING_SAVE_PATH);
	}

	private void LoadVolume() {
		float defaultVolumeDB = 0;

		foreach (var bus in Enum.GetValues(typeof(VolumeBus))) {
			float volumeDB = (float)configFile.GetValue(SOUNDMANAGER_SECTION, bus.ToString(), defaultVolumeDB);

			AudioServer.SetBusVolumeDb((int)bus, volumeDB);
		}
	}

	public float GetVolume(VolumeBus bus) {
		return AudioServer.GetBusVolumeDb((int)bus);
	}
}
