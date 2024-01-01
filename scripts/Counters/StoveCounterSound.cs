using Godot;

public partial class StoveCounterSound : AudioStreamPlayer2D {
    [Export] private StoveCounter stoveCounter;
    [Export] private Timer stoveWarningTimer;


    public override void _Ready() {
        stoveWarningTimer.Timeout += StoveWarningTimer_Timeout;
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

    }

    private void StoveWarningTimer_Timeout() {
        SoundManager.Instance.PlayWarningSound();
    }

    private void StoveCounter_OnProgressChanged(object sender, float progressNormalized) {
        float burnShowProgressAmount = .5f;

        // Timer for burning is going down instead of going up so we need to flip it
        float burningProgress = 1 - progressNormalized;

        if (stoveCounter.IsFried() && burningProgress >= burnShowProgressAmount) {

            if(stoveWarningTimer.IsStopped()) {
                stoveWarningTimer.Start();
            }
        } else {
            stoveWarningTimer.Stop();
        }
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.State e) {
        bool playSound = e == StoveCounter.State.Frying || e == StoveCounter.State.Fried;

        if (playSound) {
            if (!Playing) {
                Play();
            }
        } else {
            Stop();
        }
    }

    public override void _ExitTree() {
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged -= StoveCounter_OnProgressChanged;
    }
}
