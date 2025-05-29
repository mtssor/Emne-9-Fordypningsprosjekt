using Godot;

namespace NewGameProject.Old.Models;

/// <summary>
/// Data structure for weapon attacks.
/// Carries info for damage, knockback, position, and stun duration
/// </summary>
public class Attack
{
    public float Damage;
    public float KnockbackForce;
    public Vector2 Position;
    public float StunDuration;
}