using Godot;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Staff : Node2D
{
    [Export] public PackedScene MagicBoltScene;
    [Export] public float FireCooldown = 1.0f;

    private double _lastShotTime;

    public override void _Process(double delta)
    {
        if (!Visible)
            return;

        if (Input.IsActionJustPressed("ui_attack") && Time.GetTicksMsec() / 1000.0 - _lastShotTime >= FireCooldown)
        {
            Shoot();
            _lastShotTime = Time.GetTicksMsec() / 1000.0;
        }
    }

    private void Shoot()
    {
        if (MagicBoltScene == null)
            return;
        
        var bolt = MagicBoltScene.Instantiate() as Node2D;
        GetParent().AddChild(bolt);
        bolt.GlobalPosition = GlobalPosition;
        bolt.Rotation = Rotation;

        if (bolt is MagicBolt boltScript)
        {
            boltScript.Direction = GlobalTransform.X.Normalized();
        }
        
        var sound = GetNodeOrNull<AudioStreamPlayer2D>("MagicSound");
        sound?.Play();
    }
    
    
}