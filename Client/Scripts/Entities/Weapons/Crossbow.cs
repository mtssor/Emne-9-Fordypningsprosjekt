using Godot;
using NewGameProject.Utilities.Strategy;
using NewGameProject.Utilities.Strategy.ArrowUpgrades;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Crossbow : Node2D
{
    [Export] public PackedScene ArrowScene;
    [Export] public float FireCooldown = 0.5f;
    
    
    private double _lastShotTime;

    public override void _Process(double delta)
    {
        if (!Visible)
            return;
        
        if (Input.IsActionPressed("ui_attack") && Time.GetTicksMsec() / 1000.0 - _lastShotTime >= FireCooldown)
        {
            Shoot();
            _lastShotTime = Time.GetTicksMsec() / 1000.0;
        }
    }

    private void Shoot()
    {
        if (ArrowScene == null)
            return;
        
        Arrow arrow = ArrowScene.Instantiate() as Arrow;
        GetParent().AddChild(arrow);
        
        arrow.GlobalPosition = GlobalPosition;
        arrow.Rotation = Rotation;

        if (arrow is Arrow arrowScript)
        {
            arrowScript.Direction = GlobalTransform.X.Normalized();
        }
        
        
    }
}