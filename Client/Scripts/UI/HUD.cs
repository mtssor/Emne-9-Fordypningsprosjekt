using Godot;
using NewGameProject.Scripts.Components;

namespace NewGameProject.Scripts.UI;

public partial class HUD : Control
{
    private TextureProgressBar _healthBar;
    private WeaponUI _weaponUI;

    public override void _Ready()
    {
        _healthBar = GetNode<TextureProgressBar>("MarginContainer/Overlay/TopLeftContainer/HealthBar");
        _weaponUI = GetNode<WeaponUI>("MarginContainer/Overlay/BottomLeftContainer/WeaponUI");
    }

    public void ConnectHealth(HealthComponent healthComponent)
    {
        healthComponent.HealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(HealthUpdate healthUpdate)
    {
        if (_healthBar != null)
        {
            _healthBar.MaxValue = healthUpdate.MaxHealth;
            _healthBar.Value = healthUpdate.CurrentHealth;
        }
    }

    public void SetActiveWeapon(int weaponIndex)
    {
        _weaponUI?.SetActiveWeapon(weaponIndex);
    }
}