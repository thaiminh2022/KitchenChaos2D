using Godot;

public partial class InputManager : Node {
	private const string INPUT_MANAGER_SECTION_PREFIX = "Keybindings";

	public static InputManager Instance { get; private set; }
	public static void ResetStatic() {
		Instance = null;
	}

	private ConfigFile configFile;



	public override void _EnterTree() {
		Instance = this;

		configFile = Constants.GetSettingsConfigFile();
		Load();
	}

	public void SetKeybind(string action, InputEvent oldEvent, InputEvent @event) {
		InputMap.ActionEraseEvent(action, oldEvent);
		InputMap.ActionAddEvent(action, @event);
		Save();
	}

	/// <summary>
	///  Save the current input map to path
	/// </summary>
	private void Save() {
		foreach (string action in InputMap.GetActions()) {
			if (action.StartsWith("ui"))
				continue;

			string section = $"{INPUT_MANAGER_SECTION_PREFIX}.{action}";

			if (configFile.HasSection(section)) {
				configFile.EraseSection(section);
			}

			foreach (InputEvent keyEvent in InputMap.ActionGetEvents(action)) {
				configFile.SetValue(section, keyEvent.AsText(), keyEvent);
			}
		}

		configFile.Save(Constants.GAME_SETTING_SAVE_PATH);
	}

	/// <summary>
	/// Load the saved input map into the new input map 
	/// </summary>
	private void Load() {

		foreach (string action in InputMap.GetActions()) {
			if (action.StartsWith("ui"))
				continue;
			string section = $"{INPUT_MANAGER_SECTION_PREFIX}.{action}";

			// If the config file doesnt have any data, save the default
			if (!configFile.HasSection(section)) {
				Save();
				return;
			}

			// Remove old keybinds
			InputMap.ActionEraseEvents(action);
			
			// Add new keybinds
			foreach (string key in configFile.GetSectionKeys(section)) {
				InputEvent @event = (InputEvent)configFile.GetValue(section, key);

				InputMap.ActionAddEvent(action, @event);
			}
		}

	}
}
