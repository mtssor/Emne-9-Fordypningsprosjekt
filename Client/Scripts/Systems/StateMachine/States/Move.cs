using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

[GlobalClass]
public partial class Move : State
{
    [Export] public State IdleState;
    

    public override void Enter()
    {
        base.Enter();
        Animations.Play("Move");
    }
    
    public override State ProcessPhysics(double deltaTime)
    {
        Vector2 direction = MoveComponent.GetMovementDirection();

        if (direction == Vector2.Zero)
            return IdleState;
        
        Vector2 velocity = direction * MoveComponent.MoveSpeed;
        Parent.Velocity = velocity;
        Parent.MoveAndSlide();
        
        if (velocity.X != 0)
            Animations.FlipH = velocity.X < 0;

        return null;
    }
}