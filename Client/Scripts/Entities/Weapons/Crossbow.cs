using Godot;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Crossbow : RangedWeapon
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void Fire(Vector2 direction)
    {
        GD.Print("Crossbow is firing, direction: " + direction);
        
        if (!_canFire)
            return;

        if (ProjectileScene == null)
        {
            GD.PrintErr("ProjectileScene is not set in RangedWeapon");
            return;
        }

        var arrow = (ArrowProjectile)ProjectileScene.Instantiate();
        arrow.Launch(GlobalPosition, direction, ProjectileSpeed);
        GetTree().CurrentScene.AddChild(arrow);
        
        _canFire = false;
        GetTree().CreateTimer(Cooldown).Connect("timeout", new Callable(this, nameof(ResetFire)));
    }
}