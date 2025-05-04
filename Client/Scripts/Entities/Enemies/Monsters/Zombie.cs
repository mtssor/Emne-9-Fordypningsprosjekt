using Godot;
using NewGameProject.Scripts.Components.Interfaces;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Scripts.Entities.Enemies.Monsters;

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
        // caches key child nodes
        _animations = GetNode<AnimatedSprite2D>("Animations");
        _animatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");
		
        _stateMachine = GetNode<StateMachine>("StateMachine");
        _moveComponent = GetNode<IMoveComponent>("MoveComponent");
		
        // init state machine with Zombie dependency 
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