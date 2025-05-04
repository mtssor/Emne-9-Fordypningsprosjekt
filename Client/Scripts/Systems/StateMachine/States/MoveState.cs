using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

[GlobalClass]
public partial class MoveState : State
{
    [Export] public State IdleState { get; set; }
    
    public const float Friction = 0.15f;
    
    [Export] public float MaxSpeed { get; set; } = 200f;
    [Export] public float Acceleration { get; set; } = 40;
    private Vector2 _velocity;

    public new void Enter()
    {
        AnimationName = "Move";
        Animations.Play(AnimationName);
    }
    
    public override State ProcessPhysics(double deltaTime) {
        Vector2 direction = MoveComponent.GetMovementDirection();

        if (direction == Vector2.Zero)
            return IdleState;
        
        _velocity += direction * Acceleration;
        _velocity = _velocity.LimitLength(MaxSpeed);

        switch (_velocity.X)
        {
            case < 0 when !Animations.FlipH:
            case > 0 when Animations.FlipH:
                Animations.SpeedScale = -1;
                break;
            default:
                Animations.SpeedScale = 1;
                break;
        }
        
        //_velocity = Math.Lerp(_velocity, Vector2.Zero, Friction);
        Parent.Velocity = _velocity;
        Parent.MoveAndSlide();

        return null;
    }
}