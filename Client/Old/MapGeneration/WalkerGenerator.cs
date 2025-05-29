using Godot;
using Godot.Collections;

namespace NewGameProject.Old.MapGeneration;

// TODO: Clean-up and code more readable/simplify if possible
// TODO: Fix player node not being moved to inside the dungeon with the tiles.
/// <summary>
/// TODO: Write documentation on how to use this!
/// </summary>
[GlobalClass]
public partial class WalkerGenerator : Node2D
{
    private static PackedScene _player { get; set; }

    [Export] public float GenerationAreaWidth { get; set; } = 38f;
    [Export] private float GenerationAreaHeight { get; set; } = 21f;
    
    
    private TileMapLayer _mapLayout;
    private Rect2 _mapBounds;
    
    public override void _Ready()
    {
        _mapLayout = GetNode<TileMapLayer>("MapLayout");
        _mapBounds = new Rect2(1, 1, GenerationAreaWidth, GenerationAreaHeight);
        _player = ResourceLoader.Load<PackedScene>("uid://c0y5i5tlxl5sy");
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Array<Vector2I> FillerPoints = [];
        
        Walker walker = new(new Vector2I(19, 11), _mapBounds);
        Array<Vector2I> map = walker.Walk(200);

        for (float y = _mapBounds.Position.Y * -2; y < _mapBounds.End.Y * 2; y++)
        {
            for (float x = _mapBounds.Position.X * -2; x < _mapBounds.End.X * 2; x++)
            {
                Vector2I cellPosition = new((int)x, (int)y);
                FillerPoints.Add(cellPosition);
            }
        }
        _mapLayout.SetCellsTerrainConnect(FillerPoints, 0, 1);

        // TODO: Create Hub Room that won't be generated and supply the HubRoom's ExitDoor position as Walker's startPosition
        // Player player = _player.Instantiate<Player>();
        // AddChild(player);
        // player.GlobalPosition = walker.StartRoom.Position * 32;
        
        _mapLayout.SetCellsTerrainConnect(map, 0, 0);
    }

    private void ReloadLevel() => GetTree().ReloadCurrentScene();

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
            ReloadLevel();
    }
}