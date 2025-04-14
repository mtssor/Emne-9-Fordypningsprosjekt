using Godot;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Crossbow : RangedWeapon
{
    [Export] public PackedScene ArrowProjectileSecene { get; set; }

    public override void Fire(Vector2 direction)
    {
        if (!_canFire)
            return;

        if (ArrowProjectileSecene == null)
        {
            GD.PrintErr("ArrowProjectileSecene is not set in Crossbow");
            return;
        }

        var arrow = (ArrowProjectile)ArrowProjectileSecene.Instantiate();
        arrow.GlobalPosition = GlobalPosition;
        arrow.Direction = direction.Normalized();
        arrow.Rotation = direction.Angle();
        arrow.Speed = ProjectileSpeed;
        
        GetTree().CurrentScene.AddChild(arrow);
        
        _canFire = false;
        GetTree().CreateTimer(Cooldown).Connect("timeout", new Callable(this, nameof(ResetFire)));
    }
}