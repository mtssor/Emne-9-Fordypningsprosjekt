using Godot;

namespace NewGameProject.Utilities.StateMachine.States;

/// <summary>
/// Idle state for player or enemy state machine.
/// Plays idle animation and checks for movement input to transtion into Move state instead
/// </summary>
[GlobalClass]
public partial class Idle : State
{
    [Export] public State MoveState;

    public new void Enter()
    {
        base.Enter();
        Parent.Velocity = Vector2.Zero;
        // plays Idle animation
        Animations.Play("Idle");
    }

    public override State ProcessPhysics(double delta)
    {
        // transitions to Move state if movement input is detected 
        return MoveComponent.GetMovementDirection() != Vector2.Zero
            ? MoveState
            : null;
    }
}