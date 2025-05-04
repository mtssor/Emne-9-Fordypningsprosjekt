using System.Diagnostics;
using Godot;

namespace NewGameProject.Globals;

public partial class SceneTransistor : CanvasLayer
{
    private static string _newScene;
    private static AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public static void StartTransitionTo(string scene)
    {
        _newScene = scene;
        _animationPlayer.Play("ChangeSceneToFile");
    }

    public void ChangeSceneToFile()
    {
        bool _ = GetTree().ChangeSceneToFile(_newScene) == Error.Ok;
        Debug.Assert(_);
    }
}