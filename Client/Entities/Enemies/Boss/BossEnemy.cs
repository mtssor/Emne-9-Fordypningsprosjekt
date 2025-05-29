using Godot;
using NewGameProject.Components;

namespace NewGameProject.Entities.Enemies.Boss;

/// <summary>
/// Controls boss behavior.
/// Movement, attack, health, animation
/// </summary>
public partial class BossEnemy : CharacterBody2D
{
    [Export] public PackedScene LaserBeamScene;
    
    private BossMoveComponent _moveComponent;
    private Timer _shootTimer;
    private Node2D _laserSpawn;
    private Node2D _player;

    public override void _Ready()
    {
        _moveComponent = GetNode<BossMoveComponent>("BossMoveComponent");
        _shootTimer = GetNode<Timer>("ShootTimer");
        _laserSpawn = GetNode<Node2D>("LaserSpawn");

        _shootTimer.Timeout += ShootLaser;
        
        _player = GetTree().Root.GetNodeOrNull<Node2D>("GameScene/Player");

        if (_player == null)
            GD.PrintErr("Boss: Player not found");

        var attackZone = GetNode<Area2D>("AttackZone");
        attackZone.Connect("body_entered", new Callable(this, nameof(OnPlayerTouch)));
    }

    private void OnPlayerTouch(Node body)
    {
        if (body.HasNode("HealthComponent"))
        {
            var health = body.GetNodeOrNull<HealthComponent>("HealthComponent");
            health?.Damage(5);
            GD.Print($"Boss hit {body.Name} and dealt 5 damage");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_moveComponent == null)
            return;

        Vector2 velocity = _moveComponent.GetMovementDirection() * _moveComponent.MoveSpeed;
        Velocity = velocity;
        MoveAndSlide();

        if (GetSlideCollisionCount() > 0)
        {
            _moveComponent.Bounce();
        }
    }

    private void ShootLaser()
    {
        GD.Print("Boss is trying to shoot laser");
        
        if (LaserBeamScene == null || _player == null)
            return;

        var laser = LaserBeamScene.Instantiate<Node2D>();
        GetParent().AddChild(laser);
        laser.GlobalPosition = _laserSpawn.GlobalPosition;
        
        var direction = (_player.GlobalPosition - GlobalPosition).Normalized();
        laser.Rotation = direction.Angle();

        if (laser.HasMethod("SetDirection"))
        {
            laser.Call("SetDirection", direction);
        }

        if (laser is LaserBeam laserScript)
        {
            laserScript.SetDirection(direction);
            laserScript.Shooter = this;
        }
    }
}