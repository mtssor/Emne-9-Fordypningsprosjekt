using System;
using Godot;
using NewGameProject.Scripts.Systems.StateMachine;

namespace NewGameProject.Entities.Enemies.Goblin;

public partial class Goblin : CharacterBody2D
{
    [Export] public int ProjectileSpeed = 150;
    
    public AnimatedSprite2D Animations { get; private set; }
    public AnimatedSprite2D AnimatedEffects { get; private set; }

    public StateMachine StateMachine { get; private set; }
    public GoblinMoveComponent MoveComponent { get; private set; }

    public NavigationAgent2D NavigationAgent { get; private set; }
    public Timer PathTimer { get; private set; }
    public Timer AttackTimer { get; private set; }
    public RayCast2D AimRayCast { get; private set; }


    public override void _Ready() 
    {
        Animations = GetNode<AnimatedSprite2D>("Animations");
        AnimatedEffects = GetNode<AnimatedSprite2D>("AnimatedEffects");
		
        StateMachine = GetNode<StateMachine>("StateMachine");
        MoveComponent = GetNode<GoblinMoveComponent>("MoveComponent");
        NavigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        
        PathTimer = GetNode<Timer>("PathTimer");
        AimRayCast = GetNode<RayCast2D>("AimRayCast");
		
        PathTimer.Timeout += MoveComponent.OnPathTimerTimeOut;
        Connect("tree_exited", new Callable(GetParent(), "OnEnemyKilled"));

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
    
    private bool IfEnemyCanAttack(bool canAttack)
    {
        throw new NotImplementedException();
    }
}