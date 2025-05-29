using Godot;
using NewGameProject.Components.Interfaces;
using StateMachine = NewGameProject.Utilities.StateMachine.StateMachine;

namespace NewGameProject.Entities.Enemies;

public interface IEnemy
{
    AnimatedSprite2D Animations { get; }
    AnimatedSprite2D AnimatedEffects { get; }
    
    StateMachine StateMachine { get; }
    IMoveComponent MoveComponent { get; }
    
    NavigationAgent2D NavigationAgent { get; }
    Timer PathTimer { get; }

    void _Ready();
    void _Process(double delta);
    void _PhysicsProcess(double delta);
}