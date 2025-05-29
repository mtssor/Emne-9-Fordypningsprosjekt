using Godot;
using NewGameProject.Components.Interfaces;
using NewGameProject.Utilities.StateMachine;

namespace NewGameProject.Entities.Enemies.Goblin;

/// <summary>
/// Controls Goblin enemy using a state machine
/// </summary>
public partial class Goblin : CharacterBody2D
{
    [Export] public int ProjectileSpeed = 150;

    public AnimatedSprite2D Animations { get; private set; }
    public AnimatedSprite2D AnimatedEffects { get; private set; }

    public StateMachine StateMachine { get; private set; }
    public IMoveComponent MoveComponent { get; private set; }

    public override void _Ready()
    {
        // References animation and state components
        Animations = GetNode<AnimatedSprite2D>("Animations");
        AnimatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");

        StateMachine = GetNode<StateMachine>("StateMachine");
        MoveComponent = GetNode<IMoveComponent>("MoveComponent");

        Connect("tree_exited", new Callable(GetParent(), "OnEnemyKilled"));

        // Try assign player as target
        var player = GetTree().Root.GetNodeOrNull<Player.Player>("GameScene/Player");
        if (player != null && MoveComponent is Entities.Enemies.EnemyMoveComponent enemyMover)
        {
            enemyMover.SetTarget(player);
            GD.Print("Goblin: Player assigned as target.");
        }
        else
        {
            GD.PrintErr("Goblin: Failed to assign player target.");
        }

        // Initialize state machine
        StateMachine.Init(this, Animations, MoveComponent);
    }

    public override void _Process(double delta)
    {
        StateMachine.ProcessFrame(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.ProcessPhysics(delta);
    }
}