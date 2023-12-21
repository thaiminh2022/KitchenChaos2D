using Godot;


public partial class ProgressBarUI : ProgressBar
{
	private IHasProgress progressCounter;

	public override void _Ready()
	{
		Value = 0;
		progressCounter. += CuttingCounter_ProgressChanged;

		Hide();
	}

	private void CuttingCounter_ProgressChanged(float progressNormalized)
	{
		Value = progressNormalized;

		if (progressNormalized == 0f || progressNormalized == 1f)
			Hide();
		else 
			Show(); 
	}

}
