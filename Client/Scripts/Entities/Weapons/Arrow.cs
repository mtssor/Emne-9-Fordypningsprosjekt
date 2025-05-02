using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Arrow : Area2D
{
    [Export] public float Speed = 400f;
    [Export] public float Damage = 2f;
    [Export] public float StickTime = 0.2f;
    [Export] public float KnockbackForce = 50f;
    
    public Vector2 Direction = Vector2.Right;
    private bool _stuck = false;

    public override void _PhysicsProcess(double delta)
    {
        if (_stuck)
            return;
        
        Position += Direction * Speed * (float)delta;
    }

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }

    private async void OnAreaEntered(Area2D area)
    {
        if (_stuck)
            return;
        
        _stuck = true;
        
        if (area is HurtboxComponent hurtbox)
        {
            Attack attack = new()
            {
                Damage = Damage,
                KnockbackForce = KnockbackForce,
                Position = GlobalPosition,
                StunDuration = 0.5f
            };
            
            hurtbox.HandleWeaponCollision(attack);
        }
        
        SetPhysicsProcess(false);
        
        
        await ToSignal(GetTree().CreateTimer(StickTime), "timeout");
        QueueFree();
        
        
    }
}