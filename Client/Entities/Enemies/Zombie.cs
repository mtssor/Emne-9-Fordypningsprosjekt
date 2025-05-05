using Godot;
using NewGameProject.Components.Interfaces;
using NewGameProject.Utilities.StateMachine;

namespace NewGameProject.Entities.Enemies;

/// <summary>
/// Controls behavior of the Zombie enemy via a state machine
/// Handles both animations and movement
/// </summary>
public partial class Zombie : CharacterBody2D
{
    private AnimatedSprite2D _animations; // Main Zombie animations (Idle and Move)
    private AnimatedSprite2D _animatedEffects; // Other effects (getting hit and dying)

    private StateMachine _stateMachine; // controls movement logic
    private IMoveComponent _moveComponent; // direction vector 
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animations = GetNode<AnimatedSprite2D>("Animations");
        _animatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");

        _stateMachine = GetNode<StateMachine>("StateMachine");
        _moveComponent = GetNode<IMoveComponent>("MoveComponent");

        // NEW: Find player in scene
        var player = GetTree().Root.GetNodeOrNull<Player.Player>("GameScene/Player");
        if (player != null && _moveComponent is EnemyMoveComponent move)
        {
            move.SetTarget(player);
            GD.Print("Zombie: Assigned player target.");
        }
        else
        {
            GD.PrintErr("Zombie: Could not find player or move component.");
        }

        _stateMachine.Init(this, _animations, _moveComponent);
    }


    // handles physics-based movement
    public override void _PhysicsProcess(double delta) => _stateMachine.ProcessPhysics(delta);

    // handles frame-based animations
    // called every frame. "delta" is the time passed since the previous frame.
    public override void _Process(double delta)
    {
        _stateMachine.ProcessFrame(delta);
    }
}