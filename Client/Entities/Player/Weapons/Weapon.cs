using Godot;
using NewGameProject.Components;
using NewGameProject.Old.Models;

namespace NewGameProject.Entities.Player.Weapons;

public partial class Weapon : Area2D
{
    public float AttackDamage = 10f;
    public float KnockbackForce = 100f;
    public float StunDuration = 1.5f;

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    
    public void OnAreaEntered(Area2D area)
    {
        if (area is HurtboxComponent hurtbox)
        {
            Attack attack = new()
            {
                Damage = AttackDamage,
                KnockbackForce = KnockbackForce,
                Position = GlobalPosition,
                StunDuration = StunDuration
            };

            hurtbox.HandleWeaponCollision(attack);
        }
    }
}