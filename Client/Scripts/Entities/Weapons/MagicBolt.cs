using System;
using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

/// <summary>
/// Magic projectile sent by Staff that damages enemies on contact
/// </summary>
public partial class MagicBolt : Area2D
{
    [Export] public float Speed = 150; // projectile speed
    [Export] public float Damage = 4f; // specified damage
    [Export] public float ExplosionRadius = 50f; // Explosion radius on hit
    [Export] public float KnockBackForce = 40f; // specified knockback force
    [Export] public float StunDuration = 0f; // stun duration, currently set to not stun

    public Vector2 Direction = Vector2.Right;


    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    
    
    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    /// <summary>
    /// Handles collision with enemy hurtbox
    /// Applies damage then makes bolt disappear
    /// </summary>
    /// <param name="area"></param>
    private void OnAreaEntered(Area2D area)
    {
        if (area is HurtboxComponent hurtbox)
        {
            Attack attack = new()
            {
                Damage = Damage,
                KnockbackForce = KnockBackForce,
                Position = GlobalPosition,
                StunDuration = StunDuration,
            };
            
            hurtbox.HandleWeaponCollision(attack);
        }
        
        QueueFree();
    }

    // Area of effect explosion damage
    private void Explode()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = new PhysicsPointQueryParameters2D
        {
            Position = GlobalPosition,
            CollideWithAreas = true,
            CollisionMask = 3
        };
        
        var results = spaceState.IntersectPoint(query, 32);

        foreach (var result in results)
        {
            var colliderVariant = result["collider"];
            
            
            if (colliderVariant.VariantType == Variant.Type.Object) 
            {
                var colliderObject = (GodotObject)(colliderVariant);

                if (colliderObject is HurtboxComponent hurtbox)
                {
                    Attack attack = new()
                    {
                        Damage = Damage,
                        KnockbackForce = KnockBackForce,
                        Position = GlobalPosition,
                        StunDuration = StunDuration
                    };
                    
                    hurtbox.HandleWeaponCollision(attack);
                }
                
            }
        }
    }
    
}