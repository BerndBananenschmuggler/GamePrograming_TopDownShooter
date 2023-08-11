using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<object> Died;
    public event Action<float> HealthChanged;

    public float MaxHealth { get { return _maxHealth; } }
    public float CurrentHealth { get { return _currentHealth; } }

    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _currentHealth = 0;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        // Add damage and cap
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        HealthChanged?.Invoke(_currentHealth);

        //Debug.Log($"Health changed: {_currentHealth}");

        // Check for death
        if (_currentHealth <= 0)
            OnDestroyed();
    }    

    public void ApplyHeal(float heal)
    {
        // Add heal and cap
        _currentHealth = Mathf.Clamp(_currentHealth + heal, 0, _maxHealth);

        HealthChanged?.Invoke(_currentHealth);

        Debug.Log($"Health changed: {_currentHealth}");
    }

    private void OnDestroyed()
    {

        // Call Event
        Died?.Invoke(this);

        // Destroy Self
        Destroy(gameObject);
    }
}
