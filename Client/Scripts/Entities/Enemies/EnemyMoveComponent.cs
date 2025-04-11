using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Entities.Enemies;

[GlobalClass]
public partial class EnemyMoveComponent : Node, IMoveComponent
{
    public Vector2 GetMovementDirection()
    {
        throw new System.NotImplementedException();
    }
}