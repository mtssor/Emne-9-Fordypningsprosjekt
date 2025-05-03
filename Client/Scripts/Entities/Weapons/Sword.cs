using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Sword : Node2D
{
    [Export] public float Damage = 10f;
    [Export] public float Knockback = 100f;
    [Export] public float StunDuration = 1.5f;

    private Area2D _hitbox;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _hitbox = GetNode<Area2D>("Sprite2D/Hitbox");
        _animationPlayer = GetNode<AnimationPlayer>("SwordAnimation");

        _hitbox.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }

    public void Attack()
    {
        if (_animationPlayer.IsPlaying()) return;
        
        _animationPlayer.Play("sword_attack");
    }

    public void EnableHitbox()
    {
        _hitbox.Monitoring = true;
    }

    public void DisableHitbox()
    {
        _hitbox.Monitoring = false;
    }

    private void OnAreaEntered(Area2D area)
    {
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
        }
    }
}