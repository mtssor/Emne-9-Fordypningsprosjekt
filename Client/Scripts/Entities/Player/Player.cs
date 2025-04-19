using System;
using Godot;
using NewGameProject.Scripts.Entities.Player.Components;
using NewGameProject.Scripts.Entities.Weapons;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Scripts.Entities.Player;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	private AnimationPlayer _playerAnimations;
	
	public AnimatedSprite2D PlayerAnimations { get; set; }
	public StateMachine PlayerStateMachine { get; set; }
	public PlayerMoveComponent PlayerMoveComponent  { get; set; }


	private Node2D _sword;
	private Node2D _crossbow;
	private Node2D _activeWeapon;
	
	public override void _Ready()
	{
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<PlayerMoveComponent>("MoveComponent");

		_sword = GetNode<Node2D>("Weapon/Sword");
		_crossbow = GetNode<Node2D>("Weapon/Crossbow");
		_activeWeapon = _sword;
		
		
		_playerAnimations = GetNode<AnimationPlayer>("PlayerAnimations");

		PlayerStateMachine.Init(this, PlayerAnimations, PlayerMoveComponent);

		SetActiveWeapon(_sword);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.PhysicalKeycode == Key.Key1)
				SetActiveWeapon(_sword);
			else if (keyEvent.PhysicalKeycode == Key.Key2)
				SetActiveWeapon(_crossbow);
		}
		
		
		PlayerStateMachine.ProcessInput(@event);
	}

	private void SetActiveWeapon(Node2D newWeapon)
	{
		if (_activeWeapon != null) _activeWeapon.Visible = false;
		_activeWeapon = newWeapon;
		_activeWeapon.Visible = true;
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
		_activeWeapon.Rotation = currentMouseDirection.Angle();

		if (_activeWeapon == _sword && Input.IsActionJustPressed("ui_attack"))
		{
			var swordScript = _sword as Sword;
			swordScript?.Attack();
		}

		PlayerStateMachine.ProcessFrame(delta);
	}

	
}