using Godot;
using NewGameProject.Scripts.Components.Interfaces;
using NewGameProject.Scripts.Systems.StateMachine.States;

namespace NewGameProject.Scripts.Entities.Enemies;

/// <summary>
/// Enemy movement provider
/// Enemies chase the player if certain criteria are met 
/// </summary>
[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{
    
    [Export] public float MoveSpeed { get; set; } = 60f; // enemy movement speed
    [Export] public float ChaseRadius { get; set; } = 200f; // radius for when enemy detects player
    [Export] public int LineOfSightMask { get; set; } = 1; // Line of sight collision mask

    private Node2D _target; // enemies target (player)
    private Node2D _owner; // references the specific enemy this component is attached to
    private Stunnable _stunnable; // checks if enemy is stunned

    public override void _Ready()
    {
        _owner = GetParent<Node2D>();
        _stunnable = _owner.GetNodeOrNull<Stunnable>("Stunnable");
    }

    /// <summary>
    /// Assigns a target for enemy to chase
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Node2D target)
    {
        _target = target;
        GD.Print($"Target assigned to EnemyMoveComponent: {target?.Name}");
    }

    /// <summary>
    /// Returns the direction the enemy should move in
    /// Will return Vector2.Zero if the target is out of range or out of line-of-sight
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMovementDirection()
    {
        if (_target == null || _owner == null)
            return Vector2.Zero;
        
        // stops enemy from moving if stunned
        if (_stunnable != null && _stunnable.IsStunned)
            return Vector2.Zero;
        
        // stops enemy from chasing player if outside specified radius
        var toPlayer = _target.GlobalPosition - _owner.GlobalPosition;
        if (toPlayer.Length() > ChaseRadius)
            return Vector2.Zero;

        // stops enemy from chasing player if outside LOS
        if (!HasLineOfSight())
            return Vector2.Zero;
        
        return toPlayer.Normalized();
    }

    /// <summary>
    /// Uses raycast to determine if enemy can see the player or not 
    /// </summary>
    /// <returns></returns>
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

        // If the ray hits something that isnt the player, dont chase
        if (result.Count > 0)
        {
            var collider = result["collider"];
            if (collider.AsGodotObject() != _target)
                return false;
        }
        
        return true;
    }

}
