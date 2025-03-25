using Godot;
using NewGameProject.Scripts.Components.Interfaces;

namespace NewGameProject.Scripts.Systems.StateMachine;

[GlobalClass]
public partial class StateMachine : Node
{
    [Export] public State StartingState;
    private State _currentState;
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

    public void ChangeState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void ProcessInput(InputEvent @event)
    {
        State newState = _currentState?.ProcessInput(@event);
        if (newState != null) 
            ChangeState(newState);
    }
    
    public void ProcessFrame(double deltaTime)
    {
        State newState = _currentState?.ProcessFrame(deltaTime);
        if (newState != null)
            ChangeState(newState);
    }

    public void ProcessPhysics(double deltaTime)
    {
        State newState = _currentState?.ProcessPhysics(deltaTime);
        if (newState != null)
            ChangeState(newState);
    }
}