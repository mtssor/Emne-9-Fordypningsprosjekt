using Godot;
using NewGameProject.Scripts.Systems.Utilities;

namespace NewGameProject.Scripts.Components;


/// <summary>
/// Component for managing health
/// </summary>
[GlobalClass]
public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler(HealthUpdate healthUpdate); 
    [Signal]
    public delegate void DiedEventHandler();
    
    [Export]
    private bool _suppressDamageFloat;
    
    private float _currentHealth = 10f;
    private float _maxHealth = 10f;
    private bool _hasDied;

    // max health handling
    [Export]
    public float MaxHealth
    {
        get => _maxHealth;
        private set
        {
            _maxHealth = value;
            if (CurrentHealth > _maxHealth)
                CurrentHealth = _maxHealth;
        }
    }

    // current health values, updates when changed 
    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            float previousHealth = _currentHealth;
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            HealthUpdate healthUpdate = new()
            {
                PreviousHealth = previousHealth,
                CurrentHealth = _currentHealth,
                MaxHealth = MaxHealth,
                HealthPercentage = MaxHealth > 0 ? _currentHealth / MaxHealth : 0f,
                IsHeal = previousHealth <= _currentHealth
            };
            
            EmitSignal(SignalName.HealthChanged, healthUpdate);
            
            // signals that a unit has died 
            if (!HasHealthRemaining && !_hasDied)
            {
                _hasDied = true;
                EmitSignal(SignalName.Died);
            }
        }
    }

    public bool IsDamaged => CurrentHealth < MaxHealth;
    public bool HasHealthRemaining => !Mathf.IsEqualApprox(CurrentHealth, 0f);



    public override void _Ready() => InitializeHealth();
    private void InitializeHealth() => CurrentHealth = MaxHealth;
    
    // sets new max health value
    public void SetMaxHealth(float newMaxHealth) => MaxHealth = newMaxHealth;

    // handles the damage. Health is reduced by the amount of damage taken
    public void Damage(float damage)
    {
        GD.Print($"Health -> Current: {_currentHealth}, Loss: {damage} ");
        CurrentHealth -= damage;
    }
    
    // applies heal by effectively applying negative damage 
    public void Heal(float heal) => Damage(-heal);
}

// handles all the different kinds of health change signals. 
public partial class HealthUpdate : RefCounted
{
    public float PreviousHealth;
    public float CurrentHealth;
    public float MaxHealth;
    public float HealthPercentage;
    public bool IsHeal;
}