using Godot;
using HealthUpdate = NewGameProject.Components.HealthUpdate;

namespace NewGameProject.Globals;

public partial class SavedData : Node
{
    public static int NumberFloor { get; set; }
    public HealthUpdate Health { get; set; }

    public override void _Ready()
    {
        
    }

    private static void ResetData()
    {
        NumberFloor = 0;
    }
}