using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Entities.Player.Components;

public partial class PlayerMoveComponent : Node, IMoveComponent
{
    public Vector2 GetMovementDirection() => Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
}