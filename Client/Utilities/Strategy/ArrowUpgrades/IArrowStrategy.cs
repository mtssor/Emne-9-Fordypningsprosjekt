using Arrow = NewGameProject.Entities.Player.Weapons.Arrow;

namespace NewGameProject.Utilities.Strategy.ArrowUpgrades;

public interface IArrowStrategy
{
    void ApplyUpgrade(Arrow arrow);
}