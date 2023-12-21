using Godot;

public partial class SelectedCounterVisual : Node2D
{
	[Export] private BaseCounter clearCounter;
	public override void _Ready()
	{
		Player.Instance.SelectedCounterChange += OnCounterChange;
	}

	private void OnCounterChange(BaseCounter selectedCounter)
	{
		if (selectedCounter == clearCounter)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

}
