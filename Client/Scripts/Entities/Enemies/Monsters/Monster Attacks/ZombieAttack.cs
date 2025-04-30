using System.Collections.Generic;
using Godot;
using NewGameProject.Scripts.Components;

namespace NewGameProject.Scripts.Entities.Enemies.Monsters.Monster_Attacks;

public partial class ZombieAttack : Area2D
{
    [Export] public float Damage = 5f;
    [Export] public float HitCooldown = 0.5f;
    
    private readonly Dictionary<Node, double> _lastHitTime = new();

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body == null || !body.HasNode("HealthComponent"))
            return;
        
        var health = body.GetNode<HealthComponent>("HealthComponent");
        if (health == null)
            return;
        
        double currentTime = Time.GetTicksMsec() / 1000.0;

        if (_lastHitTime.TryGetValue(body, out double lastHit))
        {
            if (currentTime - lastHit < HitCooldown)
                return;
        }
        
        health.Damage(Damage);
        GD.Print($"Zombie attacked player for {Damage} damage");
        
        _lastHitTime[body] = currentTime;
       
    }
}