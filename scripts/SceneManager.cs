using Godot;
using Godot.Collections;
using System.IO;
using System.Linq;

public partial class SceneManager : Node {

	public static SceneManager Instance { get; private set; }

	[Export] private PackedScene[] scenes;
	private Node CurrentScene;

	// Loading scene
	[ExportGroup("Interactive settings")]
	[Export] private PackedScene loadingScene;
	[Export] private bool useSubThread = true;

	private string scenePath;
	private Array progressArray;
	private LoadingScene currentLoadingScene;


	[Signal]
	public delegate void ProgressChangedEventHandler(float progress);
	[Signal]
	public delegate void SceneLoadFinishedEventHandler();

	public enum Scene {
		Unknow = -1,
		MainMenu, 
		Game,
	}


	public override void _EnterTree() {
		Instance = this;

	}

	public override void _Ready() {
		Viewport root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
	}

	#region Switch Scene non interactive

	public static void SwitchScene(Scene scene) {
		Instance.CallDeferred(MethodName.SwitchSceneDeferred, (int)scene);
	}

	private void SwitchSceneDeferred(int index) {
		if (index >= scenes.Length || index < 0) {
			GD.PrintErr("Scene index not found");
			return;
		}

		CurrentScene.Free();
		var nextScene = GD.Load<PackedScene>(scenes[index].ResourcePath);
		CurrentScene = nextScene.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().CurrentScene = CurrentScene;

		
	}

    #endregion

    #region Switch Scene Interactive


    public void SwitchSceneInteractive(Scene scene) {
		scenePath = scenes[(int)scene].ResourcePath;

		currentLoadingScene = loadingScene.Instantiate<LoadingScene>();
		GetTree().Root.AddChild(currentLoadingScene);

		StartLoad();
	}

	private void StartLoad() {

		Error loadState = ResourceLoader.LoadThreadedRequest(scenePath, "", useSubThread);
		if (loadState == Error.Ok) {
			SetProcess(true);
		}
	}

	public override void _Process(double delta) {
		
		var loadStatus = ResourceLoader.LoadThreadedGetStatus(scenePath, progressArray);

		switch (loadStatus) {
			case ResourceLoader.ThreadLoadStatus.InvalidResource:
			case ResourceLoader.ThreadLoadStatus.Failed:
				SetProcess(false);
				break;

			case ResourceLoader.ThreadLoadStatus.InProgress:
				EmitSignal(SignalName.ProgressChanged, progressArray[0]);
				break;
			case ResourceLoader.ThreadLoadStatus.Loaded:
				var loaded_resource = ResourceLoader.LoadThreadedGet(scenePath) as PackedScene;
				EmitSignal(SignalName.ProgressChanged, 1.0);

				currentLoadingScene.SetFinishedAnimationCallBack(() => {
					EmitSignal(SignalName.SceneLoadFinished);
					GetTree().ChangeSceneToPacked(loaded_resource);

					currentLoadingScene.QueueFree();
				});

				break;
		}

	}
    #endregion

}
