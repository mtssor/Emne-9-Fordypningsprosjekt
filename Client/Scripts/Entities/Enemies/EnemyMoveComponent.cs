using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Entities.Enemies;

[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{
    
    [Export] public float MoveSpeed { get; set; } = 60f;
    [Export] public float ChaseRadius { get; set; } = 200f;
    [Export] public int LineOfSightMask { get; set; } = 1;

    private Node2D _target;
    private Node2D _owner;

    public override void _Ready()
    {
        _owner = GetParent<Node2D>();
    }

    public void SetTarget(Node2D target)
    {
        _target = target;
        GD.Print($"Target assigned to EnemyMoveComponent: {target?.Name}");
    }

    public Vector2 GetMovementDirection()
    {
        if (_target == null || _owner == null)
            return Vector2.Zero;
        
        var toPlayer = _target.GlobalPosition - _owner.GlobalPosition;
        if (toPlayer.Length() > ChaseRadius)
            return Vector2.Zero;

        if (!HasLineOfSight())
            return Vector2.Zero;
        
        return toPlayer.Normalized();
    }

    private bool HasLineOfSight()
    {
        if (_target == null || _owner == null)
            return false;

        var spaceState = _owner.GetWorld2D().DirectSpaceState;

        var query = PhysicsRayQueryParameters2D.Create(
            from: _owner.GlobalPosition,
            to: _target.GlobalPosition
        );
        query.CollisionMask = (uint)LineOfSightMask;
        query.CollideWithAreas = false;

        var result = spaceState.IntersectRay(query);

        if (result.Count > 0)
        {
            var collider = result["collider"];
            if (collider.AsGodotObject() != _target)
                return false;
        }
        
        return true;
    }

}