using Godot;

namespace NewGameProject.Scripts.Entities.Weapons;

/// <summary>
/// Crossbow logic for spawning and firing Arrow projectiles
/// </summary>
public partial class Crossbow : Node2D
{
    [Export] public PackedScene ArrowScene; // uses the arrow scene to fire arrows
    [Export] public float FireCooldown = 0.5f; // cooldown before crossbow can fire again

    private double _lastShotTime; // timestamp for when crossbow was last fired

    public override void _Process(double delta)
    {
        if (!Visible)
            return;
        
        // Checks if both attack input is pressed, and cooldown has passed
        if (Input.IsActionPressed("ui_attack") && Time.GetTicksMsec() / 1000.0 - _lastShotTime >= FireCooldown)
        {
            Shoot();
            _lastShotTime = Time.GetTicksMsec() / 1000.0;
        }
    }

    /// <summary>
    /// Spawns arrow projectile from the Arrow scene
    /// Sets its direction 
    /// </summary>
    private void Shoot()
    {
        if (ArrowScene == null)
            return;
        
        var arrow = ArrowScene.Instantiate() as Node2D;
        GetParent().AddChild(arrow);
        arrow.GlobalPosition = GlobalPosition;
        arrow.Rotation = Rotation;

        // applies the direction it will travel
        if (arrow is Arrow arrowScript)
        {
            arrowScript.Direction = GlobalTransform.X.Normalized();
        }
        
        // plays crossbow firing sound when attacking
        var sound = GetNodeOrNull<AudioStreamPlayer2D>("CrossbowFireSound");
        sound?.Play();
        GD.Print("Playing weapon sound");

        
    }
}