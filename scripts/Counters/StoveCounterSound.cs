using Godot;

public partial class StoveCounterSound : AudioStreamPlayer2D {
	[Export] private StoveCounter stoveCounter;

	public override void _Ready() {
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
	}

	private void StoveCounter_OnStateChanged(object sender, StoveCounter.State e) {
		bool playSound = e == StoveCounter.State.Frying || e == StoveCounter.State.Fried;

		if (playSound) {
			if(!Playing) {
				Play();
			}
		} else {
			Stop();
		}
	}
}
