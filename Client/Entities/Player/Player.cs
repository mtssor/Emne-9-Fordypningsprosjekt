using System.Collections.Generic;
using Godot;
using NewGameProject.Entities.Player.Weapons;
using NewGameProject.Scripts.Systems.StateMachine;
using NewGameProject.Utilities.Strategy;

namespace NewGameProject.Entities.Player;

[GlobalClass]
public partial class Player : CharacterBody2D
{
	
	public AnimatedSprite2D PlayerAnimations { get; set; }
	public StateMachine PlayerStateMachine { get; set; }
	public PlayerMoveComponent PlayerMoveComponent  { get; set; }


	private Node2D _sword;
	private Node2D _crossbow;
	private Node2D ActiveWeapon { get; set; }
	public List<BaseWeaponStrategy> Upgrades { get; set; } = [];

	public override void _Ready()
	{
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<PlayerMoveComponent>("MoveComponent");

		_sword = GetNode<Node2D>("Weapon/Sword");
		_crossbow = GetNode<Node2D>("Weapon/Crossbow");
		ActiveWeapon = _sword;

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
		if (ActiveWeapon != null) ActiveWeapon.Visible = false;
		ActiveWeapon = newWeapon;
		ActiveWeapon.Visible = true;
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
		ActiveWeapon.Rotation = currentMouseDirection.Angle();

		if (ActiveWeapon == _sword && Input.IsActionJustPressed("ui_attack"))
		{
			var swordScript = _sword as Sword;
			swordScript?.Attack();
		}

		PlayerStateMachine.ProcessFrame(delta);
	}

	
}