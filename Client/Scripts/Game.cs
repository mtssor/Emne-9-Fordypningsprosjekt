using Godot;
using NewGameProject.Scripts.Entities.Enemies;
using NewGameProject.Scripts.Entities.Enemies.Monsters;
using NewGameProject.Scripts.Entities.Player;

namespace NewGameProject.Scripts;

public partial class Game : Node2D
{
    public override void _Ready()
    {
        var player = GetNodeOrNull<Player>("Player");
        if (player == null)
        {
            GD.PrintErr("Player not found");
            return;
        }

        foreach (var enemyNode in GetTree().GetNodesInGroup("enemies"))
        {
            if (enemyNode is Node zombie)
            {
                var moveComponent = zombie.GetNodeOrNull<EnemyMoveComponent>("MoveComponent");
                moveComponent?.SetTarget(player);
                
                GD.Print("Assigned player to enemy");
            }
        }
    }

}