using System;
using Godot;
using Godot.Collections;

namespace NewGameProject.Scripts.Systems.DungeonGeneration;

/// <summary>
/// TODO: DO NOT RUN, does not work!
/// </summary>
public partial class DungeonGenerator : Node2D
{
    private TileMapLayer MapLayout;
    
    private Random _random;
    
    public const int Width = 160;
    public const int Height = 120;
    public const int CellSize = 10;
    public const int MinRoomSize = 10;
    public const int MaxRoomSize = 30;
    public const int MaxRoomCount = 20;

    private readonly int[,] _grid = new int[Width, Height];
    private readonly Array<Rect2> _rooms = [];
    private int _roomCount;
    
    private string _drawDungeon = "";

    public override void _Ready()
    {
        _random = new Random();
        
        MapLayout = GetNode<TileMapLayer>("MapLayout");
        
        InitializeGrid();
        GenerateDungeon();
        QueueRedraw();
    }

    /// <summary>
    /// TODO: Write documentation on how to use this!
    /// </summary>
    private void InitializeGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                _grid[x, y] = 1;
            }
        }
    }

    /// <summary>
    /// TODO: Write documentation on how to use this!
    /// </summary>
    private void GenerateDungeon()
    {
        for (int i = 0; i < MaxRoomCount; i++)
        {
            Rect2 room = GenerateRoom();

            if (CanPlaceRoom(room))
            {
                if (_roomCount > 0)
                    ConnectRooms(_rooms[_roomCount - 1], room);

                _rooms.Add(room);
                _roomCount++;
            }
        }
    }
    
    
    private Rect2 GenerateRoom()
    {
        int width = _random.Next() % (MaxRoomSize - MinRoomSize + 1) + MinRoomSize;
        int height = _random.Next() % (MaxRoomSize - MinRoomSize + 1) + MinRoomSize;
        int x = _random.Next() % (Width - width - 1) + 1;
        int y = _random.Next() % (Height - height - 1) + 1;
        
        return new Rect2(x, y, width, height);
    }

    private bool CanPlaceRoom(Rect2 room)
    {
        int posX = (int)room.Position.X;
        int posY = (int)room.Position.Y;
        int sizeX = (int)room.Size.X;
        int sizeY = (int)room.Size.Y;

        for (int x = posX; x < sizeX; x++)
        {
            for (int y = posY; y < sizeY; y++)
            {
                if (_grid[x, y] == 0)
                    return false;
            }
        }

        for (int x = posX; x < sizeX; x++)
        {
            for (int y = posY; y < sizeY; y++)
                _grid[x, y] = 0;
        }
        
        return true;
    }

    /// <summary>
    /// TODO: Doesn't work!
    /// </summary>
    /// <param name="roomA"></param>
    /// <param name="roomB"></param>
    /// <param name="corridorWidth"></param>
    private void ConnectRooms(Rect2 roomA, Rect2 roomB, int corridorWidth = 4)
    {
        Vector2 current = new(
            roomA.Position.X + roomA.Size.X / 2,
            roomA.Position.Y + roomA.Size.Y / 2);
        Vector2 target = new(
            roomB.Position.X + roomB.Size.X / 2,
            roomB.Position.Y + roomB.Size.Y / 2);

        while ((int)current.X != (int)target.X)
        {
            current.X += target.X > current.X ? 1 : -1;
            CarveBlock((int)current.X, (int)current.Y, corridorWidth);
        }

        while ((int)current.Y != (int)target.Y)
        {
            current.Y += target.Y > current.Y ? 1 : -1;
            CarveBlock((int)current.X, (int)current.Y, corridorWidth);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    /// <param name="corridorWidth"></param>
    private void CarveBlock(int currentX, int currentY, int corridorWidth)
    {
        int radius = corridorWidth / 2;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int x = currentX + dx, y = currentY + dy;
                if (x is >= 0 and < Width && y is >= 0 and < Height)
                    _grid[x, y] = 0;
            }
        }
    }
    
    public override void _Draw()
    {
        Array<Vector2I> floorCells = [];
        Array<Vector2I> wallCells = [];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2I cellPosition = new(x, y);

                if (_grid[x, y] != 0)
                    continue;
                
                floorCells.Add(cellPosition);
                
                if (_grid[x + 1, y] == 1)
                    wallCells.Add(cellPosition);
                if (_grid[x - 1, y] == 1)
                    wallCells.Add(cellPosition);
                
                if (_grid[x , y + 1] == 1)
                    wallCells.Add(cellPosition);
                if (_grid[x , y - 1] == 1)
                    wallCells.Add(cellPosition);
                
                if (_grid[x + 1, y + 1] == 1)
                    wallCells.Add(cellPosition);
                if (_grid[x - 1, y - 1] == 1)
                    wallCells.Add(cellPosition);

                if (_grid[x + 1, y - 1] == 1)
                    wallCells.Add(cellPosition);
                if (_grid[x - 1, y + 1] == 1)
                    wallCells.Add(cellPosition);
            }
        }
        
        MapLayout.SetCellsTerrainConnect(floorCells, 0, 0);
        MapLayout.SetCellsTerrainConnect(wallCells, 0, 1);
    }
}