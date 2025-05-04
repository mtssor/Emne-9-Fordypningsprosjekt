using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using static System.Diagnostics.Debug;

namespace NewGameProject.Old.MapGeneration;

// TODO: Make corridors instead of just walking
// TODO: Add a function that creates a wall surrounding the rooms and corridors.


/// <summary>
/// TODO: Write documentation on how to use this!
/// </summary>
[GlobalClass]
public partial class Walker : Node
{
    private readonly Random _random;
    
    private static readonly Vector2I[] Directions = 
        [ 
            Vector2I.Up, 
            Vector2I.Down, 
            Vector2I.Left, 
            Vector2I.Right 
        ];

    private Vector2I Position { get; set; } = Vector2I.Zero;
    public Room StartRoom { get; private set; }
    private Vector2I Direction  { get; set; } = Vector2I.Right;
    private Rect2 Borders { get; set; }

    private Array<Vector2I> StepHistory  { get; } = [];
    private int StepsSinceTurn { get; set; }

    private List<Room> Rooms  { get; set; } = [];
    public Func<Room, int, IEnumerable<RoomTypes>> AllowedRoomTypesRule { get; set; }

    public Walker(Vector2I startPosition, Rect2 newBorders, int? seed = null)
    {
        Assert(newBorders.HasPoint(startPosition));
        Position = startPosition;
        StepHistory.Add(Position);
        Borders = newBorders;
        
        _random = seed.HasValue ? new Random(seed.Value) : new Random();
    }

    public Walker()
    {
    }

    public Array<Vector2I> Walk(int steps)
    {
        PlaceRoom(Position);

        for (int i = 0; i < steps; i++)
        {
            if (StepsSinceTurn >= 6)
                ChangeDirection();
            if (Step())
                StepHistory.Add(Position);
            
            else ChangeDirection();
        }
        return StepHistory;
    }

    private bool Step()
    {
        Vector2I target = Position + Direction;
        if (Borders.HasPoint(target))
        {
            StepsSinceTurn++;
            Position = target;
            return true;
        }
        
        return false;
    }

    private void ChangeDirection()
    {
        PlaceRoom(Position);
        StepsSinceTurn = 0;
        
        Array<Vector2I> directions = new(Directions);
        directions.Remove(Direction);
        Shuffle(directions);

        foreach (Vector2I point in directions.Where(point => Borders.HasPoint(Position + point)))
        {
            Direction = point;
            return;
        }
    }

    private void Shuffle(Array<Vector2I> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private Room CreateRoom(Vector2I position, Vector2I size)
    {
        return new Room
        {
            Position = position, 
            Size = size
        };
    }

    private void PlaceRoom(Vector2I center)
    {
        int width = _random.Next(2, 6);
        int height = _random.Next(2, 6);

        Vector2I size = new(width, height);
        Vector2I topLeft = (center - size / 2).Abs();

        Room room = CreateRoom(center, size);

        if (Rooms.Count == 0 || StepHistory.Count == 0)
        {
            StartRoom = room;
            StartRoom.RoomType = RoomTypes.Entrance;
        }
        else
        {
            IEnumerable<RoomTypes> allowedRoomTypes = AllowedRoomTypesRule?.Invoke(room, Rooms.IndexOf(room))
                                                      ?? [RoomTypes.Normal, RoomTypes.Treasure];
            RoomTypes[] typesArray = allowedRoomTypes as RoomTypes[] ?? allowedRoomTypes.ToArray();
            RoomTypes chosenType = typesArray[_random.Next(typesArray.Length)];
            
            room.RoomType = chosenType;
        }

        Rooms.Add(room);

        for (int y = 0; y < size.Y; y++)
        {
            for (int x = 0; x < size.X; x++)
            {
                Vector2I cell = topLeft + new Vector2I(x, y);
                if (Borders.HasPoint(cell))
                    StepHistory.Add(cell);
            }
        }
    }

    public Room GetEndRoom()
    {
        if (Rooms.Count == 0)
            return null;
        
        Room endRoom = Rooms[0];
        Rooms.RemoveAt(0);
        
        Vector2I startPosition = StepHistory[0];
        foreach (Room room in Rooms.Where(room => startPosition.DistanceTo(room.Position) > 
                                                  startPosition.DistanceTo(endRoom.Position)))
        { endRoom = room; }

        endRoom.RoomType = RoomTypes.Exit;
        return endRoom;
    }
}