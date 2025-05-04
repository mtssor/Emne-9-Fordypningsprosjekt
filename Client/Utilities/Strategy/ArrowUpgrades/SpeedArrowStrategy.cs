using Godot;
using Arrow = NewGameProject.Entities.Player.Weapons.Arrow;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

[GlobalClass]
public partial class SpeedArrowStrategy : BaseWeaponStrategy, IArrowStrategy
{
    [Export] public float SpeedUpgrade = 50.0f;
    public void ApplyUpgrade(Arrow arrow) => arrow.Speed = SpeedUpgrade;
}