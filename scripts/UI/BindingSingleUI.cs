using Godot;

public partial class BindingSingleUI : HBoxContainer {
	[Export] private Label bindingText;
	[Export] private Button bindingBtn;
	private string action;
	private InputEvent currentInputEvent;

	public void SetBindingData(string action, InputEvent inputEvent) {
		this.action = action;
		this.currentInputEvent = inputEvent;

		bindingText.Text = action;
		bindingBtn.Text = inputEvent.GetPrettyText();
		 
	}
	public override void _Ready() {
		bindingBtn.Toggled += BindingBtn_Toggled;
		bindingBtn.GuiInput += BindingBtn_GuiInput;
		bindingBtn.FocusExited += BindingBtn_FocusExited;
	}

	private void BindingBtn_FocusExited() {
		bindingBtn.ButtonPressed = false;
	}

	private void BindingBtn_GuiInput(InputEvent @event) {

		if(!bindingBtn.ButtonPressed) {
			return;
		}

        bool matchedInputType =
            @event is InputEventKey || @event is InputEventJoypadButton || @event is InputEventJoypadMotion;

        if (matchedInputType) {
            InputManager.Instance.SetKeybind(action, currentInputEvent, @event);

            SetBindingData(action, @event);
            bindingBtn.ButtonPressed = false;
        }
    }

	private void BindingBtn_Toggled(bool buttonPressed) {
		bindingBtn.SetProcessUnhandledInput(buttonPressed);

		if (buttonPressed) {
			GD.Print("Binding button: ", action, " ", currentInputEvent.AsText());

			bindingBtn.Text = "...Awaiting Input...";
			ReleaseFocus();
		} else {
			GrabFocus();
		}
	}
}
