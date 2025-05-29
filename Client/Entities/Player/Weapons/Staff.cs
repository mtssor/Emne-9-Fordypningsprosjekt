using Godot;

namespace NewGameProject.Entities.Player.Weapons;

/// <summary>
/// Ranged weapon that fires magic bolts 
/// </summary>
public partial class Staff : Node2D
{
    [Export] public PackedScene MagicBoltScene; // uses the magic bolts scene to fire them
    [Export] public float FireCooldown = 1.0f; // cooldown between attacks

    private double _lastShotTime; // timestamp since last attack

    public override void _Process(double delta)
    {
        if (!Visible)
            return;

        // Fires if attack input is pressed and cooldown has passed
        if (Input.IsActionJustPressed("ui_attack") && Time.GetTicksMsec() / 1000.0 - _lastShotTime >= FireCooldown)
        {
            Shoot();
            _lastShotTime = Time.GetTicksMsec() / 1000.0;
        }
    }

    /// <summary>
    /// Instansiates the magic bolt and assigns its travel direction
    /// </summary>
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
        
        // sound effect when firing
        var sound = GetNodeOrNull<AudioStreamPlayer2D>("MagicSound");
        sound?.Play();
    }
    
    
}