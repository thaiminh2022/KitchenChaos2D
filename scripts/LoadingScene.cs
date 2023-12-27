using Godot;
using System;

public partial class LoadingScene : Node {

	[Export] private LoadingSceneUI loadingSceneUI;
	[Export] private bool useAnimation;

	Action finishAnimationCallback;
	private bool startWaitAnimationFinished = false;



	public override void _Process(double delta) {
		if(!startWaitAnimationFinished) {
			return;
		}

		if(loadingSceneUI.IsAnimationFinished()) {
			finishAnimationCallback.Invoke();
			startWaitAnimationFinished = false;
		}
	}

	public void SetFinishedAnimationCallBack(Action finishAnimationCallback) {
		this.finishAnimationCallback = finishAnimationCallback;
		
		// Loaded finishes
		if(!useAnimation) {
			// We are not waiting for any loading animation, we invoke the final events
			finishAnimationCallback();
				
		}else {
			// We are waiting for the animation to finish
			startWaitAnimationFinished = true;
		}

	}
}
