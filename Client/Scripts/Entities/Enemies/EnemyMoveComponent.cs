using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Entities.Enemies;

[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{
    [Export] public NodePath TargetPath;
    private Node2D _target;

    public override void _Ready()
    {
        if (TargetPath != null && HasNode(TargetPath))
        {
            _target = GetNode<Node2D>(TargetPath);
        }
    }

    public void SetTarget(Node2D target)
    {
        _target = target;
    }
    
    
    
    public Vector2 GetMovementDirection()
    {
        if (_target == null)
            return Vector2.Zero;
        
        var parent = GetParent<Node2D>();
        return (_target.GlobalPosition - parent.GlobalPosition).Normalized();
    }
}