using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

[GlobalClass]
public partial class Move : State
{
    [Export] public State IdleState;
    [Export] public float MoveSpeed { get; set; } = 200f;
    
    public override State ProcessPhysics(double deltaTime)
    {
        Vector2 direction = MoveComponent.GetMovementDirection();

        if (direction == Vector2.Zero)
            return IdleState;
        
        Vector2 velocity = direction * MoveSpeed;

        switch (velocity.X)
        {
            case < 0 when !Animations.FlipH:
            case > 0 when Animations.FlipH:
                Animations.SpeedScale = -1;
                break;
            default:
                Animations.SpeedScale = 1;
                break;
        }
        
        Parent.Velocity = velocity;
        Parent.MoveAndSlide();

        return null;
    }
}