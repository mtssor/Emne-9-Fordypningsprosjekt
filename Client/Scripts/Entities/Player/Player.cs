using System;
using Godot;
using NewGameProject.Scripts.Entities.Player.Components;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Scripts.Entities.Player;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	private AnimationPlayer _playerAnimations;
	
	public AnimatedSprite2D PlayerAnimations { get; set; }
	public StateMachine PlayerStateMachine { get; set; }
	public PlayerMoveComponent PlayerMoveComponent  { get; set; }


	public Node2D Weapon { get; set; }
	
	public override void _Ready()
	{
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<PlayerMoveComponent>("MoveComponent");

		Weapon = GetNode<Node2D>("Weapon");
		_playerAnimations = GetNode<AnimationPlayer>("PlayerAnimations");

		PlayerStateMachine.Init(this, PlayerAnimations, PlayerMoveComponent);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		PlayerStateMachine.ProcessInput(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		PlayerStateMachine.ProcessPhysics(delta);
	}

	public override void _Process(double delta)
	{
		Vector2 currentMouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		PlayerAnimations.FlipH = currentMouseDirection.X switch
		{
			> 0 when PlayerAnimations.FlipH => false,
			< 0 when !PlayerAnimations.FlipH => true,
			_ => PlayerAnimations.FlipH
		};
		
		// Weapon
		Weapon.Rotation = currentMouseDirection.Angle();
		if (Input.IsActionPressed("ui_attack") && !_playerAnimations.IsPlaying())
			_playerAnimations.Play("sword");
		
		PlayerStateMachine.ProcessFrame(delta);
	}

	
}