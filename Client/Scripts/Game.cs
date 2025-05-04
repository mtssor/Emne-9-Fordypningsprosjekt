using Godot;
using NewGameProject.Scripts.Entities.Enemies;
using NewGameProject.Scripts.Entities.Enemies.Monsters;
using NewGameProject.Scripts.Entities.Player;

namespace NewGameProject.Scripts;

/// <summary>
/// Controller for game world.
/// Connects enemies to the player, sets up scene logic
/// </summary>
public partial class Game : Node2D
{
    public override void _Ready()
    {
        // Finds player instance in the scene
        var player = GetNodeOrNull<Player>("Player");
        if (player == null)
        {
            GD.PrintErr("Player not found");
            return;
        }

        // Assigns the player as a target to all anamies in the "enemies" group
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