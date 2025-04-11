using Godot;
using NewGameProject.Scripts.Entities.Player;
using NewGameProject.Scripts.Entities.Weapons;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Components;

[GlobalClass]
public partial class HurtboxComponent : Area2D
{
    public const string GroupEnemyHurtbox = "enemy_hurtbox";

    [Signal]
    public delegate void HitByWeaponEventHandler();

    private AnimatedSprite2D _animations;
    private AnimatedSprite2D _animatedEffects;

    private HealthComponent _healthComponent;

    public override void _Ready()
    {
        _healthComponent = Owner.GetNode<HealthComponent>("HealthComponent");
		
        _animations = Owner.GetNode<AnimatedSprite2D>("Animations");
        _animatedEffects = Owner.GetNode<AnimatedSprite2D>("AnimatedEffects");
        
        _healthComponent.Died += OnDeath;
    }

    public void HandleWeaponCollision(Attack attack)
    {
        _healthComponent?.Damage(attack.Damage);
        if (_healthComponent is { HasHealthRemaining: true })
            _animatedEffects.Play("Hit");
    }
    
    private void OnDeath()
    {
        _animations.Visible = false;
        _animatedEffects.Play("Death");
        _animatedEffects.AnimationFinished += () => Owner.QueueFree();
    }
}