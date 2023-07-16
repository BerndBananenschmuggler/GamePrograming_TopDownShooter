using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDestroyed;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public void ApplyDamage(float damage)
    {
        // Add damage and cap
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

        // Check for death
        if (_currentHealth <= 0)
            OnDestroy();

    }    

    public void ApplyHeal(float heal)
    {
        // Add heal and cap
        _currentHealth = Mathf.Clamp(_currentHealth + heal, 0, _maxHealth);
    }

    private void OnDestroy()
    {
        // Call Event
        OnDestroyed?.Invoke();

        // Destroy Self
        Destroy(gameObject);
    }
}
