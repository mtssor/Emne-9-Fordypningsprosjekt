using Godot;

namespace NewGameProject.Scripts.Systems.MapGeneration;

public partial class ExitDoor : Area2D
{
    [Signal]
    public delegate void LeavingLevelEventHandler();
    [Signal]
    public delegate bool UnlockedDoorEventHandler();
    
    private bool _hasKey;

    public bool HasKey
    {
        get => _hasKey;
        private set
        {
            if (_hasKey == false)
            {
                
            }
        }
    }

    public void OnExitDoorEntered(Area2D body)
    {
        if (_hasKey) EmitSignal(SignalName.LeavingLevel, body);
    }
}