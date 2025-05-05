using System.Collections.Generic;
using Godot;
using NewGameProject.Utilities.Strategy;
using HealthUpdate = NewGameProject.Components.HealthUpdate;

namespace NewGameProject.Globals;

public partial class SavedData : Node
{
    public static int NumberFloor { get; set; }
    public static HealthUpdate Health { get; set; }
    public static List<float> AllDamageUp = [];
    public static List<BaseWeaponStrategy> WeaponUpgrades { get; set; }

    public override void _Ready()
    {
        
    }

    private static void ResetData()
    {
        NumberFloor = 0;
    }
}