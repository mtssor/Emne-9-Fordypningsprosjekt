using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

/// <summary>
/// Script for arrow projectiles fired from crossbow weapon, damages enemies
/// </summary>
public partial class Arrow : Area2D
{
    [Export] public float Speed = 400f; // Arrow projectile speed
    [Export] public float Damage = 2f; // Specified damage dealt to enemies
    [Export] public float StickTime = 0.2f; // Time the arrow "sticks" to enemies before disappearing
    [Export] public float KnockbackForce = 50f; // specified knockback force
    
    public Vector2 Direction = Vector2.Right; // travel direction
    private bool _stuck = false; // checks if arrow has hit something

    public override void _PhysicsProcess(double delta)
    {
        if (_stuck)
            return;
        
        // moves arrow in specified direction 
        Position += Direction * Speed * (float)delta;
    }

    public override void _Ready()
    {
        // Checks for collision with enemy
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }

    /// <summary>
    /// Called when arrow collides with another Area2D (enemy, wall)
    /// Deals damage if target hit has a hurtbox
    /// </summary>
    /// <param name="area"></param>
    private async void OnAreaEntered(Area2D area)
    {
        if (_stuck)
            return;
        
        _stuck = true; // prevents arrows from colliding multiple times
        
        if (area is HurtboxComponent hurtbox)
        {
            // constructs attack
            Attack attack = new()
            {
                Damage = Damage,
                KnockbackForce = KnockbackForce,
                Position = GlobalPosition,
                StunDuration = 0.5f
            };
            
            hurtbox.HandleWeaponCollision(attack);
        }
        
        // stops movement, and removes the arrow after a delay 
        SetPhysicsProcess(false);
        await ToSignal(GetTree().CreateTimer(StickTime), "timeout"); 
        QueueFree(); 
        
        
    }
}
