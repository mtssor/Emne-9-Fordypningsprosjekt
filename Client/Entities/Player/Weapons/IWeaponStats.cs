using Godot;

namespace NewGameProject.Entities.Player.Weapons;

public interface IWeaponStats
{
    float Damage { get; set; }
    float Speed { get; set; }
    float KnockbackForce { get; set; }
    float StunDuration { get; set; }
    float Cooldown { get; set; }
    
    
}