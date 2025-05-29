using Godot;
using NewGameProject.Components;

namespace NewGameProject.Entities.Enemies.Boss;

public partial class LaserBeam : Area2D
{
    [Export] public float Speed = 200f;
    [Export] public float Damage = 1f;
    
    
    private Vector2 _direction = Vector2.Right;

    public Node Shooter;

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.Normalized();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * Speed * (float)delta;
    }

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (Shooter != null && body == Shooter)
            return;
        
        if (body.HasNode("HealthComponent"))
        {
            var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");
            if (health != null)
            {
                health?.Damage(Damage);
                GD.Print($"Laser hit {body.Name} for {Damage} damage");
            }
            
        }
        
        QueueFree();
    }
}