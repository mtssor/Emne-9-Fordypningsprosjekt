using Godot;

namespace NewGameProject.Scripts.UI;

public partial class WeaponUI : Control
{
    private TextureRect _weaponIcon1;
    private TextureRect _weaponIcon2;
    private TextureRect _weaponIcon3;
    
    private Color _activeColor = new Color(1, 1, 1);
    private Color _inactiveColor = new Color(0.5f, 0.5f, 0.5f);

    public override void _Ready()
    {
        _weaponIcon1 = GetNodeOrNull<TextureRect>("HBoxContainer/WeaponSlot1/WeaponIcon1");
        _weaponIcon2 = GetNodeOrNull<TextureRect>("HBoxContainer/WeaponSlot2/WeaponIcon2");
        _weaponIcon3 = GetNodeOrNull<TextureRect>("HBoxContainer/WeaponSlot3/WeaponIcon3");

        if (_weaponIcon1 != null && _weaponIcon2 != null)
        {
            SetActiveWeapon(1);
        }
        else
        {
            GD.PrintErr("WeaponUI: One or more weapon icons not found in the scene!");
        }
    }


    public void SetActiveWeapon(int weaponIndex)
    {
        // debug
        GD.Print($"SetActiveWeapon called with index {weaponIndex}");
        
        if (_weaponIcon1 == null || _weaponIcon2 == null || _weaponIcon3 == null)
        {
            GD.PrintErr("WeaponUI: Attempt to set active weapon before icons initialized.");
            return;
        }
        
        
        _weaponIcon1.Modulate = weaponIndex == 1 ? _activeColor : _inactiveColor;
        _weaponIcon2.Modulate = weaponIndex == 2 ? _activeColor : _inactiveColor;
        _weaponIcon3.Modulate = weaponIndex == 3 ? _activeColor : _inactiveColor;
    }
}