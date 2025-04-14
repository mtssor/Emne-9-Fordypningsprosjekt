using Godot;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class ArrowProjectile : Area2D
{
    [Export] public Vector2 Direction { get; set; }
    [Export] public float Speed { get; set; } = 600f;

    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is NewGameProject.Scripts.Components.HurtboxComponent hurtbox)
        {
            Attack attack = new Attack
            {
                Damage = 15f,
                KnockbackForce = 50f,
                Position = GlobalPosition,
                StunDuration = 0.5f
            };
            
            hurtbox.HandleWeaponCollision(attack);
            QueueFree();
        }
    }

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
}