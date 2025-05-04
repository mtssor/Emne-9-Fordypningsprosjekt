using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Entities.Enemies;

[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{
    public Vector2 GetMovementDirection()
    {
        throw new System.NotImplementedException();
    }

    public float MoveSpeed { get; }
}