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


	public Node2D Weapon { get; set; }
	public Node2D Crossbow { get; set; }

	private Node2D _activeWeapon;
	
	public override void _Ready()
	{
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<PlayerMoveComponent>("MoveComponent");

		Node2D weaponContainer = GetNode<Node2D>("Weapon");
		Weapon = weaponContainer.GetNode<Node2D>("Sword");
		Crossbow = weaponContainer.GetNode<Node2D>("Crossbow");

		_activeWeapon = Weapon;
		Weapon.Visible = false;
		Crossbow.Visible = false;
		
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

		// Weapons
		if (Input.IsActionJustPressed("weapon_sword"))
		{
			//_activeWeapon.Visible = false;
			_activeWeapon = Weapon;
			//_activeWeapon.Visible = true;
		}
		else if (Input.IsActionJustPressed("weapon_crossbow"))
		{
			//_activeWeapon.Visible = false;
			_activeWeapon = Crossbow;
			//_activeWeapon.Visible = true;
		}
		
		
		
		if (Input.IsActionJustPressed("ui_attack"))
		{
			_activeWeapon.Visible = true;
			
			if (_activeWeapon is Crossbow crossbow)
			{
				crossbow.Fire(currentMouseDirection);
			}
			else
			{
				if (!_playerAnimations.IsPlaying())
					_playerAnimations.Play("sword");
			}
			
			GetTree().CreateTimer(0.2f).Connect("timeout", new Callable(this, nameof(HideWeapon)));
		}
		
		PlayerStateMachine.ProcessFrame(delta);
	}

	private void HideWeapon()
	{
		_activeWeapon.Visible = false;
	}

	
}