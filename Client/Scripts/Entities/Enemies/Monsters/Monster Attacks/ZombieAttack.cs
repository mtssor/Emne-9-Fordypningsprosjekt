using System.Collections.Generic;
using Godot;
using NewGameProject.Scripts.Components;

namespace NewGameProject.Scripts.Entities.Enemies.Monsters.Monster_Attacks;


/// <summary>
/// Handles the Zombie enemy melee attack logic
/// Deals damage to player when the zombies hitbox overlaps with players hitbox (HealthComponent)
/// </summary>
public partial class ZombieAttack : Area2D
{
    [Export] public float Damage = 2f; // damage dealth per hit from zombie
    [Export] public float HitCooldown = 0.5f; // cooldown preventing hits from happening repeatedly
    [Export] public float AttackStartDelay = 0.5f; // small delay before attacks become active, for spawn
    
    private readonly Dictionary<Node, double> _lastHitTime = new(); // keeps track of when each zombie last got hit
    private double _startTime; // timestamp for when attack started
    private Node _owner; 

    public override void _Ready()
    {
        _owner = GetOwner();
        // connects collision
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        _startTime = Time.GetTicksMsec() / 1000.0;
    }

    private void OnBodyEntered(Node body)
    {
        double currentTime = Time.GetTicksMsec() / 1000.0;
        
        if (currentTime - _startTime < AttackStartDelay)
            return;
        
        // makes sure body is valid and has a HealthComponent
        if (body == null || !body.HasNode("HealthComponent"))
            return;
        
        // makes it so zombies cant damage themselves
        if (body == _owner)
            return;
        
        var health = body.GetNode<HealthComponent>("HealthComponent");
        if (health == null)
            return;
        
        
        
        // cooldown on hit per target
        if (_lastHitTime.TryGetValue(body, out double lastHit))
        {
            if (currentTime - lastHit < HitCooldown)
                return;
        }
        
        // apply damage
        health.Damage(Damage);
        GD.Print($"Zombie attacked player for {Damage} damage");
        
        _lastHitTime[body] = currentTime;
       
    }
}