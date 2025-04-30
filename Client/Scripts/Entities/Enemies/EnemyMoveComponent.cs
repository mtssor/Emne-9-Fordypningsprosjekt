using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Entities.Enemies;

[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{

    [Export] public float MoveSpeed { get; set; } = 60f;

    private Node2D _target;

    public void SetTarget(Node2D target)
    {
        _target = target;
        GD.Print($"Target assigned to EnemyMoveComponent: {target?.Name}");
    }

    public Vector2 GetMovementDirection()
    {
        if (_target == null)
            return Vector2.Zero;
        
        var parent = GetParent<Node2D>();
        if (parent == null)
            return Vector2.Zero;
        
        return (_target.GlobalPosition - parent.GlobalPosition).Normalized();
    }

}