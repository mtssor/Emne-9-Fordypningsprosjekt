using Godot;

namespace NewGameProject.Scripts.Components;

[GlobalClass]
public partial class Hitbox : Area2D
{
    [Export] public float Damage = 10f;

    public Hitbox()
    {
        CollisionLayer = 2;
        CollisionMask = 0;
    }
}