using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

/// <summary>
/// Move state in player or enemies state machine.
/// Handles directional input, updates velocity, plays movement animations, and flips sprites depending on direction
/// </summary>
[GlobalClass]
public partial class Move : State
{
    [Export] public State IdleState; // returns to Idle is no movement input is detected 
    

    public override void Enter()
    {
        base.Enter();
        // plays movement animation
        Animations.Play("Move");
    }
    
    public override State ProcessPhysics(double deltaTime)
    {
        // gets directional input from movement component
        Vector2 direction = MoveComponent.GetMovementDirection();

        // no input = idle
        if (direction == Vector2.Zero)
            return IdleState;
        
        // calcs velocity based on speed and direction
        Vector2 velocity = direction * MoveComponent.MoveSpeed;
        Parent.Velocity = velocity;
        
        // moves character
        Parent.MoveAndSlide();
        
        // flips the sprite left or right depending on movement direction
        if (velocity.X != 0)
            Animations.FlipH = velocity.X < 0;

        return null;
    }
}