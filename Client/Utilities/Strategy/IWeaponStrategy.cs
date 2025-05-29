using NewGameProject.Entities.Player.Weapons;

namespace NewGameProject.Utilities.Strategy;

public interface IWeaponStrategy
{
    void ApplyUpgrade(IWeapon weapon);
}