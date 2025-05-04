using System;
using Godot;
using Godot.Collections;
using Door = NewGameProject.Rooms.Decoration_etc.Door;

namespace NewGameProject.Rooms;

[GlobalClass]
public partial class BaseRoom : Node2D
{
    [Export] private bool _bossRoom;
    private Random _random;

    private readonly Dictionary<string, PackedScene> _enemiesToSpawn = new()
    {
        ["Goblin"] = ResourceLoader.Load<PackedScene>("res://Entities/Enemies/Goblin/Goblin.tscn"),
        ["Zombie"] = ResourceLoader.Load<PackedScene>("res://Entities/Enemies/Zombie.tscn"),
    };

    private int _numberOfEnemies;

    private TileMapLayer _tileMapLayer;
    private Node2D _entrances;
    private Node2D _doorContainer;
    private Node2D _enemyPositionsContainer;
    private Area2D _playerDetector;
    public override void _Ready()
    {
        _random = new Random();
        
        _tileMapLayer = GetNode<TileMapLayer>("Floor&Walls");
        _entrances = GetNode<Node2D>("Entrance");
        _doorContainer = GetNode<Node2D>("Doors");
        _enemyPositionsContainer = GetNode<Node2D>("EnemyPositions");
        _playerDetector = GetNode<Area2D>("PlayerDetector");
        
        _numberOfEnemies = _enemyPositionsContainer.GetChildCount();

        _playerDetector.BodyEntered += OnPlayerDetectorBodyEntered;
    }

    private void OnEnemyKilled()
    {
        _numberOfEnemies--;
        if (_numberOfEnemies == 0)
            OpenDoors();
    }

    private void OpenDoors()
    {
        foreach (Node node in _doorContainer.GetChildren())
        {
            Door child = (Door)node;
            child.Open();
        }
    }

    private void CloseEntrance()
    {
        foreach (Node child in _entrances.GetChildren())
        {
            Node2D entrance = (Node2D)child;
            GD.Print(_tileMapLayer.LocalToMap(entrance.Position));
            
            _tileMapLayer.SetCell(_tileMapLayer.LocalToMap(entrance.Position), 6, Vector2I.Zero);
        }
    }

    private void SpawnEnemies()
    {
        for (int index = 0; index < _enemyPositionsContainer.GetChildren().Count; index++)
        {
            CharacterBody2D enemy = new();

            switch (_bossRoom)
            {
                case true:
                    // enemy = EnemyScenes.SomeBoss.Instantiate<CharacterBody2D>()
                    _numberOfEnemies = 15;
                    break;
                case false when _random.Next(_enemiesToSpawn.Count) == 0:
                    enemy = _enemiesToSpawn["Zombie"].Instantiate<CharacterBody2D>();
                    break;
                case false when _random.Next(_enemiesToSpawn.Count) == 1:
                    enemy = _enemiesToSpawn["Goblin"].Instantiate<CharacterBody2D>();
                    break;
                default:
                    enemy = _enemiesToSpawn["Goblin"].Instantiate<CharacterBody2D>();
                    break;
            }

            enemy.Position = _enemyPositionsContainer.GetChild<Node2D>(index).Position;
            CallDeferred(Node.MethodName.AddChild, enemy);

            AnimatedSprite2D enemySpawnEffect = ResourceLoader.Load<PackedScene>("res://Rooms/EnemySpawnEffect.tscn").Instantiate<AnimatedSprite2D>();
            enemySpawnEffect.Position = enemy.Position;
            CallDeferred(Node.MethodName.AddChild, enemySpawnEffect);
            enemySpawnEffect.Play();
        }
    }
    
    public void OnPlayerDetectorBodyEntered(Node2D sender)
    {
        _playerDetector.QueueFree();
        if (_numberOfEnemies > 0)
        {
            CloseEntrance();
            SpawnEnemies();
        }
        else
        {
            CloseEntrance();
            OpenDoors();
        }
    }
}