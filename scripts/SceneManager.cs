using Godot;
using Godot.Collections;

public partial class SceneManager : Node {

	public static SceneManager Instance { get; private set; }

	[Export] private PackedScene[] scenes;

	// Loading scene
	[ExportGroup("Interactive settings")]
	[Export] private PackedScene loadingScene;
	[Export] private bool useSubThread = true;

	private static string currentWaitingToLoadScenePath;
	private Array progressArray;
	private static LoadingScene currentLoadingScene;

	[Signal]
	public delegate void ProgressChangedEventHandler(float progress);
	[Signal]
	public delegate void SceneLoadFinishedEventHandler();

	public enum Scene {
		MainMenu = 0,
		MainGame = 1,
	}


	public override void _EnterTree() {
		Instance = this;

		GD.Print("Scene manager entered tree");
	}

	#region Switch Scene non interactive

	public static void SwitchScene(int index) {
		Instance.CallDeferred(MethodName.SwitchSceneDeferred, index);
	}
	public static void SwitchScene(string name) {

		for (int i = 0; i < Instance.scenes.Length; i++) {
			PackedScene scene = Instance.scenes[i];

			if (scene.ResourceName == name) {
				SwitchScene(i);
				break;
			}
		}

	}

	private void SwitchSceneDeferred(int index) {
		if (index >= Instance.scenes.Length || index < 0) {
			GD.PrintErr("Scene index not found");
			return;
		}
		var scene = scenes[index];
		GetTree().ChangeSceneToPacked(scene);
	}

	#endregion



	public static void SwitchSceneInteractive(Scene scene) {
		currentWaitingToLoadScenePath = Instance.scenes[(int)scene].ResourcePath;

		currentLoadingScene = Instance.loadingScene.Instantiate<LoadingScene>();
		Instance.GetTree().Root.AddChild(currentLoadingScene);

		StartLoad();
	}

	private static void StartLoad() {

		Error loadState = ResourceLoader.LoadThreadedRequest(currentWaitingToLoadScenePath, "", Instance.useSubThread);
		if (loadState == Error.Ok) {
			Instance.SetProcess(true);
		}
	}

	public override void _Process(double delta) {
		var loadStatus = ResourceLoader.LoadThreadedGetStatus(currentWaitingToLoadScenePath, progressArray);

		switch (loadStatus) {
			case ResourceLoader.ThreadLoadStatus.InvalidResource:
			case ResourceLoader.ThreadLoadStatus.Failed:
				SetProcess(false);
				break;

			case ResourceLoader.ThreadLoadStatus.InProgress:
				EmitSignal(SignalName.ProgressChanged, progressArray[0]);
				break;
			case ResourceLoader.ThreadLoadStatus.Loaded:
				var loaded_resource = ResourceLoader.LoadThreadedGet(currentWaitingToLoadScenePath) as PackedScene;
				EmitSignal(SignalName.ProgressChanged, 1.0);

				currentLoadingScene.SetFinishedAnimationCallBack(() => {
					EmitSignal(SignalName.SceneLoadFinished);
					GetTree().ChangeSceneToPacked(loaded_resource);

					currentLoadingScene.QueueFree();
				});

				break;
		}

	}
}
