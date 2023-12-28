using Godot;
using System;

public partial class GameManager : Node {

	public static GameManager Instance { get; private set; }

	public event EventHandler OnStateChanged;
	public event EventHandler OnGamePaused;
	public event EventHandler OnGameResumed;



	private enum State {
		WaitingToStart,
		CountdownToStart,
		GamePlaying,
		GameOver
	}

	private State state;

	[Export] private Timer waitingToStartTimer;
	[Export] private Timer countdownToStart;
	[Export] private Timer gamePlayingTimer;



	public override void _EnterTree() {
		Instance = this;

		SwitchState(State.WaitingToStart);
	}

	public override void _Ready() {

		waitingToStartTimer.Timeout += WaitingToStartTimer_Timeout;
		countdownToStart.Timeout += CountdownToStartTimer_Timeout;
		gamePlayingTimer.Timeout += GamePlayingTimer_Timeout;
	}

	public override void _Process(double delta) {
		if (Input.IsActionJustPressed(Constants.PAUSE)) {
			TogglePausing();
		}
	}

	private void WaitingToStartTimer_Timeout() {
		SwitchState(State.CountdownToStart);
		countdownToStart.Start();
	}
	private void CountdownToStartTimer_Timeout() {

		SwitchState(State.GamePlaying);
		gamePlayingTimer.Start();
	}

	private void GamePlayingTimer_Timeout() {
		SwitchState(State.GameOver);
	}

	public bool IsGamePlaying() => state == State.GamePlaying;
	public bool CountdownToStartActive() => state == State.CountdownToStart;
	public bool IsGameOver() => state == State.GameOver;


	public double GetCountdownToStartTimeLeft() => countdownToStart.TimeLeft;
	public double GetGameplayTimerNormalized() => gamePlayingTimer.GetTimeProgressNormalized();


	private void SwitchState(State newState) {
		state = newState;
		OnStateChanged?.Invoke(this, EventArgs.Empty);

		GD.Print(newState);

	}

	public void TogglePausing() {
		GetTree().Paused = !GetTree().Paused;

		// Set the timescale so any shader with time will stop
		if (Engine.TimeScale == 1) {
			OnGamePaused?.Invoke(this, EventArgs.Empty);

			Engine.TimeScale = 0;
		} else {
			OnGameResumed?.Invoke(this, EventArgs.Empty);
			Engine.TimeScale = 1;
		}
	}

	public override void _ExitTree() {
		waitingToStartTimer.Timeout -= WaitingToStartTimer_Timeout;
		countdownToStart.Timeout -= CountdownToStartTimer_Timeout;
		gamePlayingTimer.Timeout -= GamePlayingTimer_Timeout;
	}

}
