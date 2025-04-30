using Godot;
using NewGameProject.Scripts.Entities.Enemies;
using NewGameProject.Scripts.Entities.Enemies.Monsters;
using NewGameProject.Scripts.Entities.Player;

namespace NewGameProject.Scripts;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        var zombie = GetNode<Zombie>("Zombie");
        var player = GetNode<Player>("Player");
        

        if (zombie != null)
        {
            var moveComponent = zombie.GetNodeOrNull<EnemyMoveComponent>("MoveComponent");

            if (moveComponent != null)
            {
                moveComponent.SetTarget(player);
                GD.Print("Assigned player to zombie");
            }
            else
            {
                GD.Print("MoveComponent not found on Zombie");
            }
        }
        else
        {
            GD.Print("Zombie node not found");
        }
    }
}