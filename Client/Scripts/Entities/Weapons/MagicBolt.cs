using System;
using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class MagicBolt : Area2D
{
    [Export] public float Speed = 150;
    [Export] public float Damage = 4f;
    [Export] public float ExplosionRadius = 50f;
    [Export] public float KnockBackForce = 40f;
    [Export] public float StunDuration = 1.0f;

    public Vector2 Direction = Vector2.Right;


    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    
    
    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

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