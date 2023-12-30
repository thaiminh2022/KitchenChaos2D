using Godot;
using System;

public partial class SettingsUI : Control {
	public static SettingsUI Instance { get; private set; }

	public static void ResetStatic() {
		Instance = null;
	}

	[Export] private Button closeBtn;

	[ExportGroup("Volume")]
	[Export] private HSlider musicVolumeSlider;
	[Export] private Label musicVolumeDisplayText;

	[Export] private HSlider vfxVolumeSlider;
	[Export] private Label vfxVolumeDisplayText;

	[ExportGroup("Bindings")]
	[Export] private PackedScene bindingTemplate;
	[Export] private Control bindingHolder;

	public override void _EnterTree() {
		Instance = this;
	}

	public override void _Ready() {
		LoadDefault();
		Hide();

		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

		musicVolumeSlider.ValueChanged += MusicVolumeSlider_ValueChanged;
		vfxVolumeSlider.ValueChanged += VfxVolumeSLider_ValueChanged;
		closeBtn.Pressed += () => {
			PauseMenuUI.Instance.Show();
			Hide();
		};
	}


	private void GameManager_OnGameResumed(object sender, EventArgs e) {
		Hide();
	}

	private void VfxVolumeSLider_ValueChanged(double value) {
		SoundManager
			.Instance
			.ChangeVolume(SoundManager.VolumeBus.VFX, Mathf.LinearToDb((float)value));

		vfxVolumeDisplayText.Text = ConvertValueToDisplay(value);
	}

	private void MusicVolumeSlider_ValueChanged(double value) {
		SoundManager
			.Instance
			.ChangeVolume(SoundManager.VolumeBus.Background, Mathf.LinearToDb((float)value));

		musicVolumeDisplayText.Text = ConvertValueToDisplay(value);

	}
	private void LoadDefault() {
		LoadSoundDefault();
		LoadBindingDefault();
	}

	private void LoadSoundDefault() {
		// Background
		float backgroundVolume = SoundManager.Instance.GetVolume(SoundManager.VolumeBus.Background);
		float volumeEnergy = Mathf.DbToLinear(backgroundVolume);

		musicVolumeSlider.Value = volumeEnergy;
		musicVolumeDisplayText.Text = ConvertValueToDisplay(volumeEnergy);

		// VFX
		float vfxVolume = SoundManager.Instance.GetVolume(SoundManager.VolumeBus.VFX);
		float vfxVolumeEnergy = Mathf.DbToLinear(vfxVolume);

		vfxVolumeSlider.Value = vfxVolumeEnergy;
		vfxVolumeDisplayText.Text = ConvertValueToDisplay(vfxVolumeEnergy);
	}

	private void LoadBindingDefault() {
		foreach (string action in InputMap.GetActions()) {
			if (action.StartsWith("ui"))
				continue;

			foreach (InputEvent keyEvent in InputMap.ActionGetEvents(action)) {
				var instance = bindingTemplate.Instantiate<BindingSingleUI>();
				instance.SetbindingText(action);
				instance.SetBindingBtnText(keyEvent.AsText());
				instance.SetBindingData(action, keyEvent);
					
				bindingHolder.AddChild(instance);
			}
		}
	}

	private string ConvertValueToDisplay(double value) {
		double displayVolume = value * 100;
		return displayVolume.ToString("###");
	}
}
