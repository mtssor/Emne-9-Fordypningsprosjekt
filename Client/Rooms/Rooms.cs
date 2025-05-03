using System;
using Godot;
using Godot.Collections;
using NewGameProject.Scripts.Systems.Utilities;

namespace NewGameProject.Rooms;

[GlobalClass]
public partial class Rooms : Node2D
{
    private Random _random;
    [Export] public int NumberOfLevels = 5;
    public CharacterBody2D Player { get; set; }

    private bool _specialRoomSpawned;
    public Node2D PreviousRoom { get; set; }

    private static readonly PackedScene[] StartRooms = [
        GD.Load<PackedScene>("res://Rooms/SpawnRooms/Prison.tscn")
    ];

    private static readonly PackedScene[] IntermediateRooms = [
        ResourceLoader.Load<PackedScene>("res://Rooms/NormalRooms/Room_01.tscn"),
        ResourceLoader.Load<PackedScene>("res://Rooms/NormalRooms/Room_02.tscn"),
        ResourceLoader.Load<PackedScene>("res://Rooms/NormalRooms/Room_03.tscn"),
        ResourceLoader.Load<PackedScene>("res://Rooms/NormalRooms/Room_04.tscn"),
    ];

    public static readonly PackedScene[] SpecialRooms = [ 
        ResourceLoader.Load<PackedScene>("res://Rooms/SpecialRooms/Treasure_01.tscn")
    ];
    public static readonly PackedScene[] EndRooms = [
        ResourceLoader.Load<PackedScene>("res://Rooms/EndRooms/EndRoom_01.tscn")
    ];
    private static readonly PackedScene[] BossRoom = [
        ResourceLoader.Load<PackedScene>("res://Rooms/BossRooms/BossRoom.tscn")
    ];

    public const int TileSize = 16;
    public const int TerrainSetIndex = 0;
    public const int FloorTerrainIndex = 0;
    public const int WallTerrainIndex = 1;


    public override void _Ready()
    {
        _random = new Random();
        Player = Owner.GetNode<CharacterBody2D>("Player");

        SavedData.NumberFloor++;
        if (SavedData.NumberFloor == 3)
            NumberOfLevels = 3;
        SpawnRooms();
    }

    private void SpawnRooms()
    {
        PreviousRoom = null;
        _specialRoomSpawned = false;

        for (int level = 0; level < NumberOfLevels; level++)
        {
            Node2D room = CreateRoom(level);
            AddChild(room);
            PositionRoom(room, level);
            PreviousRoom = room;
        }
    }

    private Node2D CreateRoom(int level)
    {
        if (level == 0)
            return InstantiateRandom(StartRooms);
        if (level == NumberOfLevels - 1)
            return InstantiateRandom(EndRooms);
        if (SavedData.NumberFloor == 3)
            return InstantiateRandom(BossRoom);

        if (!_specialRoomSpawned && (IsSpecialChance() || level == NumberOfLevels - 2))
        {
            _specialRoomSpawned = true;
            return InstantiateRandom(SpecialRooms);
        }

        return InstantiateRandom(IntermediateRooms);
    }

    private static bool IsSpecialChance() => GD.Randi() % 3 == 0;

    private Node2D InstantiateRandom(PackedScene[] roomScenes)
    {
        int idx = _random.Next() % roomScenes.Length;
        return roomScenes[idx].Instantiate<Node2D>();
    }

    private void PositionRoom(Node2D room, int level)
    {
        if (level == 0)
        {
            Vector2 startPosition = room.GetNode<Marker2D>("PlayerSpawnPosition").Position;
            Player.Position = startPosition;
            return;
        }
        
        StaticBody2D previousDoor = PreviousRoom.GetNode<StaticBody2D>("Doors/Door");
        TileMapLayer previousLayer = PreviousRoom.GetNode<TileMapLayer>("Floor&Walls");

        
#region CorridorGeneration
        
        Vector2I exitPosition = previousLayer.LocalToMap(previousDoor.Position) + Vector2I.Up * 2;
        uint corridorHeight = GD.Randi() % 5 + 2;

        Array<Vector2I> wallCells = [];
        Array<Vector2I> floorCells = [];
        for (int y = 0; y < corridorHeight; y++)
        {
            previousLayer.SetCell(exitPosition + new Vector2I(-2, -y), 2,  new Vector2I(0, 9));
            previousLayer.SetCell(exitPosition + new Vector2I(-1, -y), 3,  new Vector2I(14, 2));
            previousLayer.SetCell(exitPosition + new Vector2I(0, -y), 3,  new Vector2I(14, 2));
            previousLayer.SetCell(exitPosition + new Vector2I(1, -y), 2,  new Vector2I(0, 9));
            
            // wallCells.Add(exitPosition + new Vector2I(-2, -y));
            // floorCells.Add(exitPosition + new Vector2I(-1, -y));
            // floorCells.Add(exitPosition + new Vector2I(0, -y));
            // wallCells.Add(exitPosition + new Vector2I(1, -y));
        }

        // previousLayer.SetCellsTerrainConnect(
        //     floorCells,
        //     TerrainSetIndex,
        //     FloorTerrainIndex);
        //
        // previousLayer.SetCellsTerrainConnect(
        //     wallCells,
        //     TerrainSetIndex,
        //     WallTerrainIndex);
#endregion
        
        TileMapLayer tileMapLayer = room.GetNode<TileMapLayer>("Floor&Walls");
        Marker2D marker = room.GetNode<Marker2D>("Entrance/Marker2D");

        int entranceX = tileMapLayer.LocalToMap(marker.Position).X;
        int offsetY = tileMapLayer.GetUsedRect().Size.Y;

        room.Position = previousDoor.GlobalPosition
                        + Vector2I.Up * offsetY * TileSize
                        + Vector2.Up * (1 + corridorHeight) * TileSize
                        + Vector2.Left * entranceX * TileSize;
    }
}