using Godot;
using NewGameProject.Scripts.Entities.Weapons;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

[GlobalClass]
public partial class SpeedArrowStrategy : BaseWeaponStrategy, IArrowStrategy
{
    [Export] public float SpeedUpgrade = 50.0f;
    public void ApplyUpgrade(Arrow arrow) => arrow.Speed = SpeedUpgrade;
}