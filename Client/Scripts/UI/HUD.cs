using Godot;
using NewGameProject.Scripts.Components;

namespace NewGameProject.Scripts.UI;

/// <summary>
/// Controls the HUD (heads-up display)
/// </summary>
public partial class HUD : Control
{
    private TextureProgressBar _healthBar;
    private WeaponUI _weaponUI;

    public override void _Ready()
    {
        // node locations
        _healthBar = GetNode<TextureProgressBar>("MarginContainer/Overlay/TopLeftContainer/HealthBar");
        _weaponUI = GetNode<WeaponUI>("MarginContainer/Overlay/BottomLeftContainer/WeaponUI");
    }

    // Connects the HUD to a HealthComponent to update the Health bar when health changes
    public void ConnectHealth(HealthComponent healthComponent)
    {
        healthComponent.HealthChanged += UpdateHealthBar;
    }

    // Updates healthbar based on latest health data
    private void UpdateHealthBar(HealthUpdate healthUpdate)
    {
        if (_healthBar != null)
        {
            _healthBar.MaxValue = healthUpdate.MaxHealth;
            _healthBar.Value = healthUpdate.CurrentHealth;
        }
    }

    // Informs weapon UI of currently equipped weapon
    public void SetActiveWeapon(int weaponIndex)
    {
        _weaponUI?.SetActiveWeapon(weaponIndex);
    }
}