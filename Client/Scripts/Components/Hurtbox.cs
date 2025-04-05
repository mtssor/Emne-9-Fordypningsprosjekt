using Godot;

namespace NewGameProject.Scripts.Components;

[GlobalClass]
public partial class Hurtbox : Area2D
{
    public Hurtbox()
    {
        CollisionLayer = 0;
        CollisionMask = 2;
    }

    public override void _Ready()
    {
        Connect("area_entered", new Callable(this, "OnAreaEntered"));
    }

    public void OnAreaEntered(Hitbox hitbox)
    {
        if (hitbox is null) return;

        if (Owner.HasMethod("TakeDamage"))
            Owner.Call("TakeDamage", hitbox.Damage);
    }
}