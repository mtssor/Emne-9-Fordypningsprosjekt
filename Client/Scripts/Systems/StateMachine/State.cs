using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Systems.StateMachine;

/// <summary>
/// Base class for state behavior (Idle, Move, Attack etc)
/// Used by StateMachine for character logic
/// </summary>
[GlobalClass]
public partial class State : Node
{
    [Export] public string AnimationName; // name of animation to play when state is entered
    
    public AnimatedSprite2D Animations; // reference sprite animation
    public CharacterBody2D Parent; // reference CharacterBody2D of player or enemy
    public IMoveComponent MoveComponent;  // references movement controller component for directional input

    
    // Called when a state becomes active
    public virtual void Enter()
    {
        Animations.Play(AnimationName);
    }
    
    // called when a state is exited
    public void Exit() { }
    
    public virtual State ProcessInput(InputEvent @event) => null; // Handles input events in this state
    public virtual State ProcessFrame(double deltaTime) => null; // Handles frame updates
    public virtual State ProcessPhysics(double deltaTime) => null; // Handles physics updates
}