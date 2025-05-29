using System.Collections.Generic;
using Godot;
using NewGameProject.Entities.Player;
using NewGameProject.Globals;

namespace NewGameProject.Rooms.Decoration;

[GlobalClass]
public partial class PrisonExit : Area2D
{
	[Export] public string SceneToLoad = "GameScene";
	
	// To store all main scenes we may want to transition to upon exiting Prison
	private Dictionary<string, string> _scenes = new()
	{
		["GameScene"] = "res://Scenes/GameScene.tscn",
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnPrisonExitInput;
	}

	private void OnPrisonExitInput(Node2D body)
	{
		SceneTransistor.StartTransitionTo(_scenes[SceneToLoad]);
	}
}