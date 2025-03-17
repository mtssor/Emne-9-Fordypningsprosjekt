using RogueSharp_Tutorial.Core;
using RogueSharp_Tutorial.Interfaces;
using RogueSharp_Tutorial.Systems;
using RogueSharp;
using Path = RogueSharp.Path;

namespace RogueSharp_Tutorial.Behaviors;

public class StandardMoveAndAttack : IBehavior
{
    public bool Act(Monster monster, CommandSystem commandSystem)
    {
        DungeonMap dungeonMap = Game.DungeonMap;
        Player player = Game.Player;
        FieldOfView monsterFov = new FieldOfView(dungeonMap);
        
        // if monster has not been alerted, compute a fov
        // use the monsters Awareness value for distance in fov check
        // if player is in monsters fov, alert it 
        // add a mesage to MessageLog with this alert
        if (!monster.TurnsAlerted.HasValue)
        {
            monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
            if (monsterFov.IsInFov(player.X, player.Y))
            {
                Game.MessageLog.Add($"{monster.Name} is eager to fight {player.Name}");
                monster.TurnsAlerted = 1;
            }
        }

        if (monster.TurnsAlerted.HasValue)
        {
            // before we find a path, make sure to make the monster and player cells walkable
            dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
            dungeonMap.SetIsWalkable(player.X, player.Y, true);

            PathFinder pathFinder = new PathFinder(dungeonMap);
            Path path = null;

            try
            {
                path = pathFinder.ShortestPath(
                    dungeonMap.GetCell(monster.X, monster.Y),
                    dungeonMap.GetCell(player.X, player.Y));

            }
            catch (PathNotFoundException)
            {
                // the monster can see the player, but cant find the path
                // this could be due to other monsters blocking the way
                // add message to the MessageLog that the monster is waiting 
                Game.MessageLog.Add($"{monster.Name} waits for a turn");
            }
            
            // setting walkable back to false
            dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
            dungeonMap.SetIsWalkable(player.X, player.Y, false);
            
            // in the case there is a path, tell CommandSystem to move the monster
            if (path != null)
            {
                try
                {
                    commandSystem.MoveMonster(monster, path.Steps.First());
                }
                catch (NoMoreStepsException)
                {
                    Game.MessageLog.Add($"{monster.Name} growls in frustration");
                }
            }

            monster.TurnsAlerted++;
            
            // lose alerted status every 15 turns
            // as long as player is still in fov the monster will stay alert
            // otherwise the mosnter will quit chasing the player
            if (monster.TurnsAlerted > 15)
            {
                monster.TurnsAlerted = null;
            }
        }
        return true;
    }
}