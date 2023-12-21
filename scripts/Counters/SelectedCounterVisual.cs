using Godot;

public partial class SelectedCounterVisual : Node2D
{
	[Export] private BaseCounter clearCounter;
	public override void _Ready()
	{
		Player.Instance.OnSelectedCoutnerChange += OnCounterChange;
	}

	private void OnCounterChange(object sender, BaseCounter selectedCounter)
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
