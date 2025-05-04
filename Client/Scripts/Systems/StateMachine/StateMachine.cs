using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Systems.StateMachine;

/// <summary>
/// Finite State Machine (FSM) for player and enemies
/// Manages transitions and updates for player and enemy logic
/// </summary>
[GlobalClass]
public partial class StateMachine : Node
{
    // initial state when machine is first init
    [Export] public State StartingState;
    private State _currentState;
    
    // inits child states and sets the first one
    public void Init(CharacterBody2D owner, AnimatedSprite2D animations, IMoveComponent moveComponent)
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                state.Parent = owner;
                state.Animations = animations;
                state.MoveComponent = moveComponent;
            }
        }
        ChangeState(StartingState);
    }

    // Transitions to a new state
    public void ChangeState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    // Passes input to the current state and handles transition if needed
    public void ProcessInput(InputEvent @event)
    {
        State newState = _currentState?.ProcessInput(@event);
        if (newState != null) 
            ChangeState(newState);
    }
    
    // Passes frame update to current state, switches state if needed
    public void ProcessFrame(double deltaTime)
    {
        State newState = _currentState?.ProcessFrame(deltaTime);
        if (newState != null)
            ChangeState(newState);
    }

    // passees physics update to current state, switches state if needed
    public void ProcessPhysics(double deltaTime)
    {
        State newState = _currentState?.ProcessPhysics(deltaTime);
        if (newState != null)
            ChangeState(newState);
    }
}