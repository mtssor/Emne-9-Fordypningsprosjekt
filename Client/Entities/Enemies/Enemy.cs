using Godot;
using NewGameProject.Scripts.Components.Interfaces;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Entities.Enemies;

public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D _animations;
	private AnimatedSprite2D _animatedEffects;

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

		Connect(Node.SignalName.TreeExited, new Callable(GetParent(), "OnEnemyKilled"));
	}

	public override void _PhysicsProcess(double delta) => _stateMachine.ProcessPhysics(delta);

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_stateMachine.ProcessFrame(delta);
	}
}