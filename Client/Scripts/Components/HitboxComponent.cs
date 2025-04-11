using Godot;
using NewGameProject.Scripts.Models;

namespace NewGameProject.Scripts.Components;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent HealthComponent;
    
    public HitboxComponent()
    {
        CollisionLayer = 2;
        CollisionMask = 0;
    }

    
}