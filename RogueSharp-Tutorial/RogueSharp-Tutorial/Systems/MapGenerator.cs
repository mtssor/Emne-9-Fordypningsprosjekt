using RogueSharp_Tutorial.Core;
using RogueSharp;
using RogueSharp.DiceNotation;

namespace RogueSharp_Tutorial.Systems;

public class MapGenerator
{
    private readonly int _width;
    private readonly int _height;
    private readonly int _maxRooms;
    private readonly int _roomMaxSize;
    private readonly int _roomMinSize;
    
    private readonly DungeonMap _map;
    
    // constructing a new mapgenerator requires the dimensions of the maps it will create
    // now also the sizes and maximum number of rooms
    public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
    {
        _width = width;
        _height = height;
        _maxRooms = maxRooms;
        _roomMaxSize = roomMaxSize;
        _roomMinSize = roomMinSize;
        _map = new DungeonMap();
    }
    
    // generate a new map. Simple open floor with walls around
    public DungeonMap CreateMap()
    {
        // init every cell in the map by setting walkable, transparency, and explored to true
        _map.Initialize(_width, _height);
        
        // try to place as many rooms as the specified maxRooms
        for (int r = _maxRooms; r > 0; r--)
        {
            // determine the size and position of the room randomly
            int roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
            int roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
            int roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
            int roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);
            
            // all of our rooms can be represented as rectangles
            var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);
            
            // check to see if the room rectangle intersects with any other rooms
            bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));
            
            // as long as it doesnt intersect, add it to the list of rooms
            if (!newRoomIntersects)
            {
                _map.Rooms.Add(newRoom);
            }
        }
        
        // iterate through each room that was generated
        // dont do anything with the first room, so start at r = 1 isntead of r = 0
        for (int r = 1; r < _map.Rooms.Count; r++)
        {
            // for all remaining rooms get the center of the room and the previous room
            int previousRoomCenterX = _map.Rooms[r - 1].Center.X;
            int previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
            int currentRoomCenterX = _map.Rooms[r].Center.X;
            int currentRoomCenterY = _map.Rooms[r].Center.Y;
            
            // gives a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
            if (Game.Random.Next(1, 2) == 1)
            {
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
            }
            else
            {
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
            }
        }
        
        
        
        // iterate through each room that we wanted to place
        // call CreateRoom to make it
        // call CreateDoors to make doors
        foreach (Rectangle room in _map.Rooms)
        {
            CreateRoom(room);
            CreateDoors(room);
        }
        PlacePlayer();
        
        PlaceMonster();
        
        return _map;
        
        /*
        foreach (Cell cell in _map.GetAllCells())
        {
            _map.SetCellProperties(cell.X, cell.Y, true, true, true);
        }

        // set the first and last rows in the map to not be transparent or walkable
        foreach (Cell cell in _map.GetCellsInRows(0, _height -1))
        {
            _map.SetCellProperties(cell.X, cell.Y, false, false, true);
        }

        // set the first and last column in the map to not be transparent or walkable
        foreach (Cell cell in _map.GetCellsInColumns(0, _width -1))
        {
            _map.SetCellProperties(cell.X, cell.Y, false, false, true);
        }
        */
    }

    private void CreateRoom(Rectangle room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top + 1; y < room.Bottom; y++)
            {
                _map.SetCellProperties(x, y, true, true, false);
            }
        }
    }
    
    // carve a tunnel out of the map parallel to the x-axis
    private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
    {
        for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        {
            _map.SetCellProperties(x, yPosition, true, true);
        }
    }
    
    // carve a tunnel out of the map parallel to the y-axis
    private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
    {
        for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        {
            _map.SetCellProperties(xPosition, y, true, true);
        }
    }

    // Find the center of first room created and place Player there
    private void PlacePlayer()
    {
        Player player = Game.Player;
        if (player == null)
        {
            player = new Player();
        }
        
        player.X = _map.Rooms[0].Center.X;
        player.Y = _map.Rooms[0].Center.Y;
        
        _map.AddPlayer(player);
    }

    private void PlaceMonster()
    {
        foreach (var room in _map.Rooms)
        {
            // each room has a 60% chance of having monsters
            if (Dice.Roll("1D10") < 7 )
            {
                // generate between 1-4 monsters
                var numberOfMonsters = Dice.Roll("1D4");
                for (int i = 0; i < numberOfMonsters; i++)
                {
                    // find a random walkable location in the room to place the monster
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                    // its possible that the room doesnt have space to place a monster
                    // in that case skip creating the monster
                    if (randomRoomLocation != null)
                    {
                        // temp hard code this monster to be created at lvl 1
                        var monster = Kobold.Create(1);
                        monster.X = randomRoomLocation.X;
                        monster.Y = randomRoomLocation.Y;
                        _map.AddMonster(monster);
                    }
                }
            }
        }
    }

    private void CreateDoors(Rectangle room)
    {
        // the boundaries of the room
        int xMin = room.Left;
        int xMax = room.Right;
        int yMin = room.Top;
        int yMax = room.Bottom;
        
        // put the rooms border cells into a list
        List<Cell> borderCells = _map.GetCellsAlongLine(xMin, yMin, xMax, yMin).ToList();
        borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMin, xMax, yMax));
        borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMax, xMax, yMax));
        borderCells.AddRange(_map.GetCellsAlongLine(xMax, yMin, xMax, yMax));
        
        // go through each of the rooms boder cells and look for locations to place doors
        foreach (Cell cell in borderCells)
        {
            if (IsPotentialDoor(cell))
            {
                // a door must block fov when its closed
                _map.SetCellProperties(cell.X, cell.Y, false, true);
                _map.Doors.Add(new Door
                {
                    X = cell.X,
                    Y = cell.Y,
                    IsOpen = true
                });
            }
        }
    }
    
    // checks to see if a cell is a good candidate for placement of a door 
    private bool IsPotentialDoor(Cell cell)
    {
        // if the cell is not walkable
        // then it is a wall and not a good place for a door 
        if (!cell.IsWalkable)
        {
            return false;
        }
        
        // store references to all of the neighboring cells
        Cell right = _map.GetCell(cell.X + 1, cell.Y);
        Cell left = _map.GetCell(cell.X - 1, cell.Y);
        Cell top = _map.GetCell(cell.X, cell.Y - 1);
        Cell bottom = _map.GetCell(cell.X, cell.Y + 1);
        
        // make sure there isnt already a door there
        if (_map.GetDoor(cell.X, cell.Y) != null ||
            _map.GetDoor(right.X, right.Y) != null ||
            _map.GetDoor(left.X, left.Y) != null ||
            _map.GetDoor(top.X, top.Y) != null ||
            _map.GetDoor(bottom.X, bottom.Y) != null)
        {
            return false;
        }
        
        // this is a good place for a door on the left or right side of the room
        if (right.IsWalkable && left.IsWalkable && !top.IsWalkable && !bottom.IsWalkable)
        {
            return true;
        }
        
        // this is a good place for a door on the top or bottom of the room
        if (!right.IsWalkable && !left.IsWalkable && top.IsWalkable && bottom.IsWalkable)
        {
            return true;
        }
        
        return false;
    }
}