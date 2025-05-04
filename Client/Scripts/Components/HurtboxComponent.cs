using Godot;
using NewGameProject.Scripts.Entities.Player;
using NewGameProject.Scripts.Entities.Weapons;
using NewGameProject.Scripts.Systems.StateMachine.States;
using Attack = NewGameProject.Scripts.Models.Attack;

namespace NewGameProject.Scripts.Components;

/// <summary>
/// Defines a hurtbox. Signal created when overlapping with Hitboxes. Handles damage, stun, and death.
/// </summary>
[GlobalClass]
public partial class HurtboxComponent : Area2D
{
    public const string GroupEnemyHurtbox = "enemy_hurtbox";

    [Signal]
    public delegate void HitByWeaponEventHandler();

    private AnimatedSprite2D _animations;
    private AnimatedSprite2D _animatedEffects;

    private HealthComponent _healthComponent;

    // grabs the necessary nodes from the player or enemy 
    public override void _Ready()
    {
        _healthComponent = Owner.GetNode<HealthComponent>("HealthComponent");
		
        _animations = Owner.GetNode<AnimatedSprite2D>("Animations");
        _animatedEffects = Owner.GetNode<AnimatedSprite2D>("AnimatedEffects");
        
        // If health is 0 = death
        _healthComponent.Died += OnDeath;
    }

    public void HandleWeaponCollision(Attack attack)
    {
        GD.Print($"Hurtbox: Enemy took {attack.Damage} damage");
        
        _healthComponent?.Damage(attack.Damage);
        
        // hit feedback if still alive 
        if (_healthComponent is { HasHealthRemaining: true })
            _animatedEffects.Play("Hit");

        // apply stun (if applicable)
        if (attack.StunDuration > 0f)
        {
            var stunnable = Owner.GetNodeOrNull<Stunnable>("Stunnable");
            stunnable?.ApplyStun(attack.StunDuration);
        }
    }
    
    private void OnDeath()
    {
        GD.Print("Enemy died");
        _animations.Visible = false;
        _animatedEffects.Play("Death");
        
        // removes node when death animation finishes
        _animatedEffects.AnimationFinished += () => Owner.QueueFree();
    }
}
