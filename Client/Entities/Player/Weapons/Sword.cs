using System.Collections.Generic;
using Godot;
using NewGameProject.Components;
using NewGameProject.Globals;
using NewGameProject.Old.Models;

namespace NewGameProject.Entities.Player.Weapons;

/// <summary>
/// Players sword weapon. Handles attack animations, hit detection, applies damage, stun effects etc. to enemies
/// </summary>
public partial class Sword : Node2D
{
    [Export] public float Damage = 10f; // specified amount of damage 
    [Export] public float Knockback = 100f; // knockback force
    [Export] public float StunDuration = 1.5f; // stun duration

    private Area2D _hitbox; // detecs collision with enemy hurtbox
    private AnimationPlayer _animationPlayer; // animation player for sword attack animations (swinging)
    private HashSet<Area2D> _alreadyHit = new(); // keeps track of enemies already hit by sword to prevent multiple hits per swing

    public override void _Ready()
    {
        // finds nodes on Godot for hitbox and animations
        _hitbox = GetNode<Area2D>("Sprite2D/Hitbox");
        _animationPlayer = GetNode<AnimationPlayer>("SwordAnimation");

        // connected hit signal
        _hitbox.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        _hitbox.Monitoring = false;
        
        foreach (float damageUp in SavedData.AllDamageUp)
            Damage *= damageUp;
    }

    /// <summary>
    /// Triggers sword attack animation
    /// </summary>
    public void Attack()
    {
        if (_animationPlayer.IsPlaying()) return;
        
        // plays sound when attacking
        var sound = GetNodeOrNull<AudioStreamPlayer2D>("SwordSlashSound");
        sound?.Play();
        
        _animationPlayer.Play("sword_attack");
    }

    /// <summary>
    /// Activates the hitbox, then clears for previously hit enemies
    /// </summary>
    public void EnableHitbox()
    {
        _alreadyHit.Clear();
        _hitbox.Monitoring = true;
        GD.Print("Sword hitbox enabled");
    }

    /// <summary>
    /// Deactivates sword hitbox after attack animation ends
    /// </summary>
    public void DisableHitbox()
    {
        _hitbox.Monitoring = false;
        GD.Print("Sword hitbox disabled");
    }

    /// <summary>
    /// Checks when swords hitbox overlaps with enemy hurtbox
    /// </summary>
    /// <param name="area"></param>
    private void OnAreaEntered(Area2D area)
    {
        // prevents hitting same enemy multiple times with 1 swing
        if (_alreadyHit.Contains(area)) return;
        
        GD.Print("Sword hit something: ", area.Name);
        
        // checks for hurtbox
        if (area is HurtboxComponent hurtbox)
        {
            Attack attack = new()
            {
                Damage = Damage,
                KnockbackForce = Knockback,
                Position = GlobalPosition,
                StunDuration = StunDuration
            };

            hurtbox.HandleWeaponCollision(attack);
            
            _alreadyHit.Add(area);
        }
    }
}