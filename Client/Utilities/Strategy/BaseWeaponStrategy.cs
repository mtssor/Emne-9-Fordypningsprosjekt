using Godot;

namespace NewGameProject.Utilities.Strategy;

[GlobalClass]
public partial class BaseWeaponStrategy : Resource
{
    [Export] public Texture2D Texture;
    [Export] public string UpgradeText { get; set; }
}