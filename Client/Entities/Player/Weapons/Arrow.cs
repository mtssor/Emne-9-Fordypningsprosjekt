using Godot;
using NewGameProject.Scripts.Components;
using NewGameProject.Scripts.Models;
using NewGameProject.Utilities.Strategy;
using NewGameProject.Utilities.Strategy.ArrowUpgrades;

namespace NewGameProject.Entities.Player.Weapons;

[GlobalClass]
public partial class Arrow : Area2D
{
    [Export] public float Speed { get; set; } = 400f;
    [Export] public float Damage { get; set; } = 5f;
    [Export] public float KnockbackForce { get; set; } = 50f;
    [Export] public float StickTime = 0.2f;

    public Vector2 Direction = Vector2.Right;
    private bool _stuck = false;

    private Player _player;
    public override void _Ready()
    {
        _player = GetOwner<Player>();
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_stuck)
            return;
        
        foreach (BaseWeaponStrategy upgrade in _player.Upgrades)
        {
            if (upgrade is IArrowStrategy arrowUpgrades)
                arrowUpgrades.ApplyUpgrade(this);
        }
        
        Position += Direction * Speed * (float)delta;
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