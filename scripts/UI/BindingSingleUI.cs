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

		if (@event is InputEventKey newEvent) {
			InputManager.Instance.SetKeybind(action, currentInputEvent, newEvent);

			SetBindingData(action, newEvent);
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
