using Godot;

[GlobalClass]
public partial class FocusToParentOnVisible : Control {

    public override void _Ready() {
        (GetParent() as Control).GrabFocus();
    }
}
