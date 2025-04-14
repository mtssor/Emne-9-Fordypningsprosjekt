using Godot;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class RangedWeapon : Area2D
{
    [Export] PackedScene ProjectileScene { get; set; }
    [Export] public float ProjectileSpeed { get; set; } = 400f;
    [Export] public float Cooldown { get; set; } = 0.5f;
    
    protected bool _canFire = true;

    public override void _Ready()
    {
        
    }

    public virtual void Fire(Vector2 direction)
    {
        if (!_canFire)
        {
            return;
        }

        if (ProjectileScene == null)
        {
            GD.PrintErr("ProjectileScene is not set in RangedWeapon");
            return;
        }

        var projectile = (ArrowProjectile)ProjectileScene.Instantiate();
        projectile.GlobalPosition = GlobalPosition;
        projectile.Direction = direction.Normalized();
        projectile.Speed = ProjectileSpeed;
        
        GetTree().CurrentScene.AddChild(projectile);
        
        _canFire = false;
        GetTree().CreateTimer(Cooldown).Connect("timeout", new Callable(this, nameof(ResetFire)));
    }

    protected void ResetFire()
    {
        _canFire = true;
    }
}