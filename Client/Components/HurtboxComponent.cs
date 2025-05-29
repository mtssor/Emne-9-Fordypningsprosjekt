using Godot;
using NewGameProject.Utilities.StateMachine.States;
using Attack = NewGameProject.Old.Models.Attack;

namespace NewGameProject.Components;

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
        if (_healthComponent == null)
            GD.PrintErr($"[HurtboxComponent] Could not find HealthComponent on {Owner.Name}");
		
        _animations = Owner.GetNode<AnimatedSprite2D>("Animations");
        _animatedEffects = Owner.GetNode<AnimatedSprite2D>("AnimatedEffects");
        
        // If health is 0 = death
        _healthComponent.Died += OnDeath;
    }

    public void HandleWeaponCollision(Attack attack)
    {
        GD.Print($"Hurtbox: Enemy took {attack.Damage} damage");

        if (_healthComponent == null)
        {
            GD.PrintErr("HurtboxComponent: Missing HealthComponent!");
            return;
        }

        _healthComponent.Damage(attack.Damage);

        if (_healthComponent.HasHealthRemaining)
            _animatedEffects?.Play("Hit");

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