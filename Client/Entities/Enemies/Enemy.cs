using Godot;
using NewGameProject.Components.Interfaces;
using NewGameProject.Utilities.StateMachine;

namespace NewGameProject.Entities.Enemies;


/// <summary>
/// Class for generic enemies. Manages state machine, animations, effects etc.
/// </summary>
public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D _animations; // Main animations (idle, move)
	private AnimatedSprite2D _animatedEffects; // certain effects like getting hit or death

	private StateMachine _stateMachine;
	private IMoveComponent _moveComponent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animations = GetNode<AnimatedSprite2D>("Animations");
		_animatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");
		
		_stateMachine = GetNode<StateMachine>("StateMachine");
		_moveComponent = GetNode<IMoveComponent>("MoveComponent");
		
		_stateMachine.Init(this, _animations, _moveComponent);
	}

	public override void _PhysicsProcess(double delta) => _stateMachine.ProcessPhysics(delta);

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_stateMachine.ProcessFrame(delta);
	}
}