using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace NewGameProject.Scripts.Systems.MapGeneration;

public partial class Map : Node2D
{
    [Export] public float GenerationAreaWidth { get; set; } = 32f;
    [Export] private float GenerationAreaHeight { get; set; } = 32f;
    
    
    private TileMapLayer _mapLayout;
    private Rect2 _mapBounds;

    public Map()
    {
        _mapBounds = new Rect2(1, 1, GenerationAreaWidth, GenerationAreaHeight);
    }

    public override void _Ready()
    {
        _mapLayout = GetNode<TileMapLayer>("MapLayout");
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        Walker walker = new(new Vector2I(19, 11), _mapBounds);
        Array<Vector2I> map = walker.Walk(200);
        walker.QueueFree();
        
        _mapLayout.SetCellsTerrainConnect(map, 0, 0);
    }

    public void ReloadLevel() => GetTree().ReloadCurrentScene();

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
            ReloadLevel();
    }
}