using NewGameProject.Scripts.Entities.Weapons;

namespace NewGameProject.Utilities.Strategy;

public interface IWeaponStrategy
{
    void ApplyUpgrade(IWeapon weapon);
}