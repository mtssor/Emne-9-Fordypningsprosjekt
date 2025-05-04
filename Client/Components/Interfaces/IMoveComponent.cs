using Godot;

namespace NewGameProject.Components.Interfaces;

public interface IMoveComponent
{
    Vector2 GetMovementDirection();
    float MoveSpeed { get; }
}