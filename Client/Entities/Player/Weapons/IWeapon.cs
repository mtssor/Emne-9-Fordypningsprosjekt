namespace NewGameProject.Entities.Player.Weapons;

public interface IWeapon
{
    float Speed { get; set; }
    float Damage { get; set; }
    float KnockbackForce { get; set; }
    float StunDuration { get; set; }
}