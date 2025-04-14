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


	// references weapon nodes, children of the "Weapon" Node2D under Player
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

if (_activeWeapon != null)
        {
            _activeWeapon.Rotation = currentMouseDirection.Angle();
        }

        if (Input.IsActionJustPressed("weapon_sword"))
        {
            _activeWeapon = Weapon;
        }
        else if (Input.IsActionJustPressed("weapon_crossbow"))
        {
            _activeWeapon = Crossbow;
        }
        
        if (_activeWeapon is Crossbow crossbow)
        {
            if (Input.IsActionJustPressed("ui_attack"))
            {
                _activeWeapon.Visible = true;
                crossbow.Fire(currentMouseDirection);
                GetTree().CreateTimer(0.2f)
                    .Connect("timeout", new Callable(this, nameof(HideWeapon)));
            }
        }
        else
        {
            if (Input.IsActionPressed("ui_attack"))
            {
                _activeWeapon.Visible = true;
                var swordAnimation = _activeWeapon.GetNode<AnimationPlayer>("SwordAnimation");
                if (swordAnimation != null && !swordAnimation.IsPlaying())
                {
                    swordAnimation.Play("sword_attack");
                }
            }
            else
            {
                _activeWeapon.Visible = false;
                var swordAnimation = _activeWeapon.GetNode<AnimationPlayer>("SwordAnimation");
                if (swordAnimation != null && swordAnimation.IsPlaying())
                {
                    swordAnimation.Stop();
                }
            }
        }

        PlayerStateMachine.ProcessFrame(delta);
    }

	private void HideWeapon()
	{
		_activeWeapon.Visible = false;
	}

	
}