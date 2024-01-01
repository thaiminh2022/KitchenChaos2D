using Godot;
using System.Linq;
using System.Text;

public partial class TutorialUI : Control {
	// Move is joined for better space management
	[Export] private Label moveText;
	[Export] private Label interactText;
	[Export] private Label altInteractText;
	[Export] private Label pauseText;

	public override void _Ready() {
		InputManager.Instance.OnBindingRebind += InputManager_OnBindingRebind;
		GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
		UpdateVisual();
		Show();
	}

	private void Instance_OnStateChanged(object sender, System.EventArgs e) {
		if(GameManager.Instance.IsCountdownToStartActive()) {
			Hide();
		}
	}

	private void InputManager_OnBindingRebind(object sender, System.EventArgs e) {
		UpdateVisual();
	}

	private void UpdateVisual() {
		moveText.Text = GetMoveText();
		
		interactText.Text = 
			string.Join("", GetPrettyStrings(Constants.Bindings.Interact));
		
		altInteractText.Text = 
			string.Join("", GetPrettyStrings(Constants.Bindings.Alt_Interact));

		pauseText.Text = string.Join("", GetPrettyStrings(Constants.Bindings.Pause));

	}

	private string GetMoveText() {
		StringBuilder builder = new();

		builder.Append(string.Join("", GetPrettyStrings(Constants.Bindings.Move_Up)));
		builder.Append(string.Join("", GetPrettyStrings(Constants.Bindings.Move_Left)));
		builder.Append(string.Join("", GetPrettyStrings(Constants.Bindings.Move_Down)));
		builder.Append(string.Join("", GetPrettyStrings(Constants.Bindings.Move_Right)));


		return builder.ToString();
	}

	private string[] GetPrettyStrings(Constants.Bindings binding) {
		var arr = InputMap.ActionGetEvents(binding.ToString());

		return arr.Select(e => {
			if (!InputManager.Instance.MatchCurrentDevice(e)) {
				return string.Empty;
			}
			return e.GetPrettyText();

		}).ToArray();
	}


}
