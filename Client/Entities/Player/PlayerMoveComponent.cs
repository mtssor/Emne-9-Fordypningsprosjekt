using Godot;
using NewGameProject.Components.Interfaces;

namespace NewGameProject.Entities.Player;

/// <summary>
/// Simple player moivement input provider
/// Retruns directions based on input
/// </summary>
public partial class PlayerMoveComponent : Node, IMoveComponent
{
    [Export] public float MoveSpeed { get; set; } = 150f; // Player movement speed

    // standard input actions for movement in games. Player moves in the direction pressed (WASD and Arrow keys)
    public Vector2 GetMovementDirection() => Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
}