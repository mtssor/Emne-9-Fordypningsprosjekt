using Godot;
using NewGameProject.Scripts.Entities.Enemies;

namespace NewGameProject.Scripts;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        var player = GetNode<Node2D>("Player");
        var zombie = GetNode<Node2D>("Zombie");

        if (zombie != null)
        {
            var moveComponent = zombie.GetNodeOrNull<Node>("Zombie/MoveComponent") as EnemyMoveComponent;
            moveComponent?.SetTarget(player);
        }
    }
}