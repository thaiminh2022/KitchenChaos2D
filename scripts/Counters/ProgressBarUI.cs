using Godot;


public partial class ProgressBarUI : ProgressBar
{
	[Export] Node2D hasProgressObject;

	public override void _Ready()
	{
		if (hasProgressObject is IHasProgress hasProgress)
		{
			hasProgress.OnProgressChanged += HasProgress_ProgressChanged;
		}
		else
		{
			GD.PrintErr("The given: ", hasProgressObject.Name, " does not implement IHasProgress");
		}

		Value = 0;
		Hide();
	}

	private void HasProgress_ProgressChanged(object sender, float progressNormalized)
	{
		Value = progressNormalized;

		if (progressNormalized == 0f || progressNormalized == 1f)
			Hide();
		else
			Show();
	}

}
