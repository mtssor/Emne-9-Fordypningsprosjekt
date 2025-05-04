using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

[GlobalClass]
public partial class IdleState : State
{
    [Export] public State MoveState { get; set; }
    
    public new void Enter()
    {
        AnimationName = "Idle";
        Animations.Play(AnimationName);
        
        Parent.Velocity = Vector2.Zero;
    }

    public override State ProcessInput(InputEvent @event)
    {
        return MoveComponent.GetMovementDirection() != Vector2.Zero
            ? MoveState
            : null;
    }
}