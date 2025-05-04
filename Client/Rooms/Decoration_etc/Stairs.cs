using Godot;
using Player = NewGameProject.Entities.Player.Player;
using SceneTransistor = NewGameProject.Globals.SceneTransistor;

namespace NewGameProject.Rooms.Decoration_etc;

public partial class Stairs : Area2D
{
    [Export] public string ScenePath { get; set; } = "GameScene.tscn";
    private CollisionShape2D _collisionShape2D;

    public override void _Ready()
    {
        _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        BodyEntered += OnStairsBodyEntered;
    }

    private void OnStairsBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            _collisionShape2D.SetDeferred("disabled", true);
            SceneTransistor.StartTransitionTo($"res://Scenes/{ScenePath}");
        }
    }
}