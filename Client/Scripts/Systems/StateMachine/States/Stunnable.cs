using Godot;

namespace NewGameProject.Scripts.Systems.StateMachine.States;

/// <summary>
/// Component for stun behavior
/// Used by enemies to temporarely make them inactive after taking certain attacks
/// </summary>
public partial class Stunnable : Node   
{
    private bool _isStunned = false; // tracks if enemy is currently stunned
    private float _stunTimer = 0f; // countdown timer for stun duration
    
    public bool IsStunned => _isStunned; // returns true if enemy is stunned

    public override void _Ready()
    {
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        // decreases stun timer each frame if stunned
        if (_isStunned)
        {
            // ends stun if time has ran out
            _stunTimer -= (float)delta;
            if (_stunTimer <= 0f)
            {
                _isStunned = false;
                GD.Print("Zombie recovered from being stunned");
            }
        }
    }

    /// <summary>
    /// Triggers a stun effect on enemy for a set duration
    /// </summary>
    /// <param name="Time to remain stunned in seconds"></param>
    public void ApplyStun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
        GD.Print($"Zombie was stunned for {duration} seconds ");
    }
}