using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Health _health;

    private void Start()
    {
        _healthBarImage.fillAmount = 1f;        
    }

    private void OnEnable()
    {
        _health = GetComponent<Health>();
        if (_health == null)
            Debug.LogWarning("Health is missing");

        _health.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float value)
    {
        _healthBarImage.fillAmount = Mathf.Clamp01(value / _health.MaxHealth);
    }
}
