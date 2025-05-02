using Godot;
using NewGameProject.Scripts.Systems.Utilities;

namespace NewGameProject.Scripts.Components;

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
            if (!HasHealthRemaining && !_hasDied)
            {
                _hasDied = true;
                EmitSignal(SignalName.Died);
            }
        }
    }

    public bool IsDamaged => CurrentHealth < MaxHealth;
    public bool HasHealthRemaining => !Mathf.IsEqualApprox(CurrentHealth, 0f);

    public override void _Ready() => CallDeferred(nameof(InitializeHealth));
    private void InitializeHealth() => CurrentHealth = MaxHealth;
    
    public void SetMaxHealth(float newMaxHealth) => MaxHealth = newMaxHealth;

    public void Damage(float damage)
    {
        GD.Print($"Health -> Current: {_currentHealth}, Loss: {damage} ");
        CurrentHealth -= damage;
    }
    
    public void Heal(float heal) => Damage(-heal);
}

public partial class HealthUpdate : RefCounted
{
    public float PreviousHealth;
    public float CurrentHealth;
    public float MaxHealth;
    public float HealthPercentage;
    public bool IsHeal;
}