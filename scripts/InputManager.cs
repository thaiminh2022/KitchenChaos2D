using Godot;
using System;

public partial class InputManager : Node {

    private const string INPUT_MANAGER_SECTION_PREFIX = "Keybindings";

    public event EventHandler OnBindingRebind;


    public enum SupportedDevice {
        NOT_SUPPOTED = -1,
        KeyboardMouse,
        Controller,
    }

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

        OnBindingRebind?.Invoke(this, EventArgs.Empty);
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
    public void LoadDefault() {
        InputMap.LoadFromProjectSettings();
        Save();
    }

    public bool MatchCurrentDevice(InputEvent inputEvent) {
        return MatchSupportedDevice(inputEvent, GetConnectedDevice());
    }
    public bool MatchSupportedDevice(InputEvent inputEvent, SupportedDevice device ) {
        return GetDeviceFromInput(inputEvent) == device;
    }

    private SupportedDevice GetConnectedDevice() {
        if(Input.GetConnectedJoypads().Count > 0) {
            return SupportedDevice.Controller;
        }
        else {
            return SupportedDevice.KeyboardMouse;
        }
    }
    private SupportedDevice GetDeviceFromInput(InputEvent inputEvent) {
        if (inputEvent is InputEventKey|| inputEvent is InputEventMouse) {
            return SupportedDevice.KeyboardMouse;
        }
        if (inputEvent is InputEventJoypadMotion 
            || inputEvent is InputEventJoypadButton b) {

            return SupportedDevice.Controller;
        }

        return SupportedDevice.NOT_SUPPOTED;

    }
}
