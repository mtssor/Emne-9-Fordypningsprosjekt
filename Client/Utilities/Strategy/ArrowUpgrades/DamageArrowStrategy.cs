using Godot;
using NewGameProject.Scripts.Entities.Weapons;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

[GlobalClass]
public partial class DamageArrowStrategy : BaseWeaponStrategy, IArrowStrategy
{
    [Export] private float _damage = 5.0f;
    public void ApplyUpgrade(Arrow arrow) => arrow.Damage += _damage;
}