using Godot;
using System;

public partial class SettingsUI : Control {
	public static SettingsUI Instance { get; private set; }

	public static void ResetStatic() {
		Instance = null;
	}

	[Export] private Button closeBtn;
	[Export] private Button loadDefaultBtn;


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
		LoadUserSoundSettingUI();
		LoadUserBindingUI();

		GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;

		musicVolumeSlider.ValueChanged += MusicVolumeSlider_ValueChanged;
		vfxVolumeSlider.ValueChanged += VfxVolumeSLider_ValueChanged;
		
		closeBtn.Pressed += () => {
			PauseMenuUI.Instance.ShowUI();
			Hide();
		};

		loadDefaultBtn.Pressed += () => {
			InputManager.Instance.LoadDefault();
			LoadUserBindingUI();
		};

		Hide();
	}

	public void ShowUI() {
		Show();
		musicVolumeSlider.GrabFocus();
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


	private void LoadUserSoundSettingUI() {
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

	private void LoadUserBindingUI() {
		foreach (var child in bindingHolder.GetChildren()) {
			if(child is not BindingSingleUI) {
				continue;
			}
			
			child.QueueFree();
		}
		
		foreach (string action in InputMap.GetActions()) {
			if (action.StartsWith("ui"))
				continue;

			foreach (InputEvent keyEvent in InputMap.ActionGetEvents(action)) {
				
				// Hide unnessesary bindings
				if(!InputManager.Instance.MatchCurrentDevice(keyEvent)) {
					continue;
				}
				
				
				var instance = bindingTemplate.Instantiate<BindingSingleUI>();
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
