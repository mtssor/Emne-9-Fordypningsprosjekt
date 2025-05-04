using System;
using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Entities.Player.Components;
using NewGameProject.Scripts.Entities.Weapons;
using NewGameProject.Scripts.Systems.StateMachine;
using NewGameProject.Scripts.UI;

namespace NewGameProject.Scripts.Entities.Player;

/// <summary>
/// Main player controller. Handles animation, movement, input, death logic, weapon switching
/// </summary>
[GlobalClass]
public partial class Player : CharacterBody2D
{
	private AnimationPlayer _playerAnimations; // Handles certain animations like death
	
	public AnimatedSprite2D PlayerAnimations { get; set; } // references animation sprites for Idle, movement etc.
	public StateMachine PlayerStateMachine { get; set; } // State machine handling movement logic
	public PlayerMoveComponent PlayerMoveComponent  { get; set; } // Script for movement input (WASD, arrow keys)


	// available weapons for player
	private Node2D _sword;
	private Node2D _crossbow;
	private Node2D _activeWeapon;
	private Node2D _staff;
	

	// HUD instance
	[Export] private NodePath HUDPath;
	private HUD _hud;
	
	// Health system
	private HealthComponent _healthComponent;
	
	// Animations effects for getting hit, dying etc.
	private AnimatedSprite2D _animatedEffects;
	
	private bool _isDead = false;
	
	public override void _Ready()
	{
		// references key components
		PlayerAnimations = GetNode<AnimatedSprite2D>("Animations");
		_animatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");
		_playerAnimations = GetNode<AnimationPlayer>("PlayerAnimations");
		
		PlayerStateMachine = GetNode<StateMachine>("StateMachine");
		PlayerMoveComponent = GetNode<PlayerMoveComponent>("MoveComponent");

		// Loads weapons from child nodes in Godot
		_sword = GetNode<Node2D>("Weapon/Sword");
		_crossbow = GetNode<Node2D>("Weapon/Crossbow");
		_staff = GetNode<Node2D>("Weapon/Staff");
		_activeWeapon = _sword;
		
		// Init health system
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.Died += OnPlayerDied; // Death signal
		
		
		
		// Init movement and animation logic
		PlayerStateMachine.Init(this, PlayerAnimations, PlayerMoveComponent);

		
		// connects HUD
		if (HUDPath != null)
		{
			_hud = GetNode<HUD>(HUDPath);
			_hud?.ConnectHealth(_healthComponent);
		}

		// default weapon, game always starts with Sword equipped
		SetActiveWeapon(_sword);
	}
	
	
	
	/// <summary>
	/// Handles weapon switching inputs
	/// </summary>
	/// <param name="event"></param>
	public override void _UnhandledInput(InputEvent @event)
	{
		// Player swaps weapon by pressing 1, 2, 3 keys
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.PhysicalKeycode == Key.Key1)
				SetActiveWeapon(_sword);
			else if (keyEvent.PhysicalKeycode == Key.Key2)
				SetActiveWeapon(_crossbow);
			else if (keyEvent.PhysicalKeycode == Key.Key3)
				SetActiveWeapon((_staff));
		}
		
		
		PlayerStateMachine.ProcessInput(@event);
	}

	/// <summary>
	/// Makes the chosen weapon visible and hides the previous equipped one
	/// Updates HUD icon, displaying the equipped weapon
	/// </summary>
	/// <param name="newWeapon"></param>
	private void SetActiveWeapon(Node2D newWeapon)
	{
		if (_activeWeapon != null) _activeWeapon.Visible = false;
		
		_activeWeapon = newWeapon;
		_activeWeapon.Visible = true;

		if (_hud != null)
		{
			if (_activeWeapon == _sword)
				_hud.SetActiveWeapon(1);
			else if (_activeWeapon == _crossbow)
				_hud.SetActiveWeapon(2);
			else if (_activeWeapon == _staff)
				_hud.SetActiveWeapon(3);
		}
			
	}

	/// <summary>
	/// Lets state machine handle movement logic
	/// Does not process if palyer is dead
	/// </summary>
	/// <param name="delta"></param>
	public override void _PhysicsProcess(double delta)
	{
		if (_isDead)
			return;
		
		PlayerStateMachine.ProcessPhysics(delta);
	}

	/// <summary>
	/// Handles animation flipping, weapon aiming, and attack input
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		// flips player sprite to face mouse (player looks to the left or right depending on cursor location)
		Vector2 currentMouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		PlayerAnimations.FlipH = currentMouseDirection.X switch
		{
			> 0 when PlayerAnimations.FlipH => false,
			< 0 when !PlayerAnimations.FlipH => true,
			_ => PlayerAnimations.FlipH
		};
		
		// Active weapon rotates to always face cursor, so player can attack in any desired direction
		_activeWeapon.Rotation = currentMouseDirection.Angle();

		// Sword attack
		if (_activeWeapon == _sword && Input.IsActionJustPressed("ui_attack"))
		{
			var swordScript = _sword as Sword;
			swordScript?.Attack();
		}

		PlayerStateMachine.ProcessFrame(delta);
	}

	/// <summary>
	/// Called as soon as player health hits 0
	/// Stops player movement and restarts level after a short delay.
	/// </summary>
	private void OnPlayerDied()
	{
		GD.Print("Player has died");
		_isDead = true;
		
		// stops movement
		Velocity = Vector2.Zero;
		
		// Creates a timer to reload scene after a specified delay
		var timer = new Timer();
		AddChild(timer);
		timer.WaitTime = 0.5;
		timer.OneShot = true;
		timer.Timeout += () =>
		{
			GD.Print("Restarting scene");
			GetTree().ReloadCurrentScene();
		};
		
		timer.Start();
	}

	/// <summary>
	/// Path to restart scene after death animation finishes (Unused atm)
	/// </summary>
	private void OnDeathAnimationFinished()
	{
		GD.Print("Restarting scene");
		GetTree().ReloadCurrentScene();
	}

	
}
