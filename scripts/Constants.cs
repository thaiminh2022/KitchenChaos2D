using Godot;
using static Constants;
using static Godot.RenderingDevice;

public static class Constants
{

	public enum Bindings {
		Move_Left,
		Move_Right,
		Move_Up,
		Move_Down,
		Interact,
		Alt_Interact,
		Pause,
	}


	public static string GAME_SETTING_SAVE_PATH = "user://settings.cfg";
	
	public static ConfigFile GetSettingsConfigFile() {
		ConfigFile configFile = new ConfigFile();

		var err = configFile.Load(GAME_SETTING_SAVE_PATH);
		
		if(err != Error.Ok) {
			configFile.Save(GAME_SETTING_SAVE_PATH);
		}

		return configFile;
	}



	public static void SetParent(this Node node, Node newParent)
	{
		var nodeParent = node.GetParent();
		if (nodeParent is not null)
		{
			nodeParent.RemoveChild(node);
		}
		newParent.AddChild(node);

	}

	public static double GetTimeProgressNormalized(this Timer timer)
	{
		return timer.TimeLeft / timer.WaitTime;
	}
    public static string GetPrettyText(this InputEvent inputEvent) {

        if (inputEvent is InputEventKey k) {
            return k.AsTextKeycode();
        }
        if (inputEvent is InputEventJoypadMotion j) {

            return j.Axis.ToString();
        }
        if (inputEvent is InputEventJoypadButton b) {
            return b.ButtonIndex.ToString();
        }

        // If this other type, return the default.
        return inputEvent.AsText();
    }


}

