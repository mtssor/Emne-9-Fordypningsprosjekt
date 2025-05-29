using Godot;
using NewGameProject.Components.Interfaces;

namespace NewGameProject.Entities.Enemies.Boss;


/// <summary>
/// Boss movement logic.
/// Moves in random direction and bounces off walls
/// </summary>
public partial class BossMoveComponent : Node, IMoveComponent
{
    [Export] private float _moveSpeed = 100f;

    private CharacterBody2D _owner;
    private Vector2 _direction = Vector2.Zero;
    private RandomNumberGenerator _rng = new();

    public override void _Ready()
    {
        _owner = GetParent<CharacterBody2D>();
        PickNewDirection();
    }
    
    public Vector2 GetMovementDirection() => _direction;

    public void Bounce()
    {
        GD.Print("Boss hit a wall, picking a new direction");
        PickNewDirection();
    }

    private void PickNewDirection()
    {
        _direction = new Vector2(
            _rng.RandfRange(-1.0f, 1.0f),
            _rng.RandfRange(-1.0f, 1.0f)
            ).Normalized();
    }

    public float MoveSpeed => _moveSpeed;
}