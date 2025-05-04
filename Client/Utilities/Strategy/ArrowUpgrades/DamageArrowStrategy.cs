using Godot;
using Arrow = NewGameProject.Entities.Player.Weapons.Arrow;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

[GlobalClass]
public partial class DamageArrowStrategy : BaseWeaponStrategy, IArrowStrategy
{
    [Export] private float _damage = 5.0f;
    public void ApplyUpgrade(Arrow arrow) => arrow.Damage += _damage;
}