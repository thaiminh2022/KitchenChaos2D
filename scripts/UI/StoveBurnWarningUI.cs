using Godot;

public partial class StoveBurnWarningUI : TextureRect {
	private const string FLASH_WARNING_ANIMATON = "burn_warning_flash";

	[Export] private StoveCounter stoveCounter;
	[Export] private AnimationPlayer warningFlash;
	public override void _Ready() {
		stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

		Hide();
	}

	private void StoveCounter_OnProgressChanged(object sender, float progressNormalized) {
		float burnShowProgressAmount = .5f;

		// Timer for burning is going down instead of going up so we need to flip it
		float burningProgress = 1 - progressNormalized;

		if (stoveCounter.IsFried() && burningProgress >= burnShowProgressAmount) {
			Show();
			warningFlash.Play(FLASH_WARNING_ANIMATON);

		} else {
			Hide();
			warningFlash.Stop();
		}
	}
}
