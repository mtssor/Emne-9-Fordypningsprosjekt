using RLNET;
using RogueSharp;


namespace RogueSharp_Tutorial.Core;

public class DungeonMap : Map
{
    
    private readonly List<Monster> _monsters;
    public List<Rectangle> Rooms;
    public List<Door> Doors { get; set; }
    
    public DungeonMap()
    {
        _monsters = new List<Monster>();
        Rooms = new List<Rectangle>();
        Doors = new List<Door>();
    }
    
    
    // this method will be called any time we move the player to update fov
    public void UpdatePlayerFieldOfView()
    {
        Player player = Game.Player;
        // compute the fov based on the players location and awareness
        ComputeFov(player.X, player.Y, player.Awareness, true);
        // mark all cells in fov as having been explored
        foreach (Cell cell in GetAllCells())
        {
            if (IsInFov(cell.X, cell.Y))
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }
    }
    
    // returns true when able to place the Actor on the cell, false otherwise
    public bool SetActorPosition(Actor actor, int x, int y)
    {
        // only allow actor placement if the cell is walkable
        if (GetCell(x, y).IsWalkable)
        {
            SetIsWalkable(actor.X, actor.Y, true);
            // update the actors position
            actor.X = x;
            actor.Y = y;
            // the new cell the actor is on is now not walkable
            SetIsWalkable(actor.X, actor.Y, false);
            // try to open a door if one exists here
            OpenDoor(actor, x, y);
            // dont forget to update the fov if we just repositioned the player
            if (actor is Player)
            {
                UpdatePlayerFieldOfView();
            }
            return true;
        }
        return false;
        
        
    }
    
    // a helper method for setting the IsWalkable property on a cell
    public void SetIsWalkable(int x, int y, bool isWalkable)
    {
        Cell cell = GetCell(x, y);
        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
    }
    
    
    // the Draw method will be called each time the map is updated
    // It will render all the symbols/colors for each cell to the map sub consoole
    public void Draw(RLConsole mapConsole, RLConsole statConsole)
    {
        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }
        
        foreach (Door door in Doors)
        {
            door.Draw(mapConsole, this);
        }
        
        // keep an index so we know which position to draw monster stats at
        int i = 0;
        
        // iterate through each monster on the map and draw it after drawing the cells
        foreach (Monster monster in _monsters)
        {
            monster.Draw(mapConsole, this);
            // when the monster is in fov, draw their stats
            if (IsInFov(monster.X, monster.Y))
            {
                // pass in the index to DrawStats and increment it afterwards
                monster.DrawStats(statConsole, i);
                i++;
            }
        }

        
    }

    private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
    {
        // When we havent explored a cell yet, we dont want to draw anything
        if (!cell.IsExplored)
        {
            return;
        }
        
        // When a cell is currently in the fov it should be drawn with lighter colors
        if (IsInFov(cell.X, cell.Y))
        {
            //Choose the symbol to draw based on if the cell is walkable or not 
            // '.' for floor, '#' for walls
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
            }
        }
        // when a cell is outside of fov, draw it in darker colors
        else
        {
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
            }
        }
    }

    // called by MapGenerator after we generate a new map to add the player 
    public void AddPlayer(Player player)
    {
        Game.Player = player;
        SetIsWalkable(player.X, player.Y, false);
        UpdatePlayerFieldOfView();
        Game.SchedulingSystem.Add(player);
    }

    public void AddMonster(Monster monster)
    {
        _monsters.Add(monster);
        // after adding the monster to the map make sure to make the cell not walkable
        SetIsWalkable(monster.X, monster.Y, false);
        Game.SchedulingSystem.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        _monsters.Remove(monster);
        // after removing the monster from the map, make sure the cell is walkable again
        SetIsWalkable(monster.X, monster.Y, true);
        Game.SchedulingSystem.Remove(monster);
    }

    public Monster GetMonsterAt(int x, int y)
    {
        return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
    }
    
    // look for a random location in the room that is walkable
    public Point GetRandomWalkableLocationInRoom(Rectangle room)
    {
        if (DoesRoomHaveWalkableSpace(room))
        {
            for (int i = 0; i < 100; i++)
            {
                int x = Game.Random.Next(1, room.Width - 2) + room.X;
                int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                if (IsWalkable(x, y))
                {
                    return new Point(x, y);
                }
            }
        }
        
        //if we didnt find a walkable location in the room return null
        return null;
    }
    
    // iterate through each cell in the room and return true if any are walkable
    public bool DoesRoomHaveWalkableSpace(Rectangle room)
    {
        for (int x = 1; x <= room.Width - 2; x++)
        {
            for (int y = 1; y < room.Height - 2; y++)
            {
                if (IsWalkable(x + room.X, y + room.Y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // return door at x, y position, or null if one is not found
    public Door GetDoor(int x, int y)
    {
        return Doors.FirstOrDefault(d => d.X == x && d.Y == y);
    }
    
    // actor opens door located at x, y position
    private void OpenDoor(Actor actor, int x, int y)
    {
        Door door = GetDoor(x, y);
        if (door != null && !door.IsOpen)
        {
            door.IsOpen = true;
            var cell = GetCell(x, y);
            // once door is opened it should be marked transparent, no longer blocking fov
            SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);
            
            Game.MessageLog.Add($"{actor.Name} opened a door");
        }
    }
    
    
    
    
}