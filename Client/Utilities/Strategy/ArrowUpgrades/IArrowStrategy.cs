using NewGameProject.Scripts.Entities.Weapons;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

public interface IArrowStrategy
{
    void ApplyUpgrade(Arrow arrow);
}