using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

[GlobalClass]
public partial class Idle : State
{
    [Export] public State MoveState;

    public new void Enter()
    {
        base.Enter();
        Parent.Velocity = Vector2.Zero;
    }

    public override State ProcessInput(InputEvent @event)
    {
        return MoveComponent.GetMovementDirection() != Vector2.Zero 
            ? MoveState 
            : null;
    }
}