using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    private Transform _canvasTransform;
    private Health _health;
    private Camera _camera;

    [SerializeField] private bool _isEnemyControlled = false;

    private void Start()
    {
        _healthBarImage.fillAmount = 1f;   
        _camera = Camera.main;
        if (_isEnemyControlled)
            _canvasTransform = _healthBarImage.transform.parent.transform;
    }

    private void OnEnable()
    {
        _health = GetComponent<Health>();
        if (_health == null)
            Debug.LogWarning("Health is missing");

        _health.HealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= HandleHealthChanged;
    }

    private void Update()
    {
        if (_isEnemyControlled)
            _canvasTransform.LookAt(_canvasTransform.position + _camera.transform.forward);
    }

    private void HandleHealthChanged(float value)
    {
        _healthBarImage.fillAmount = Mathf.Clamp01(value / _health.MaxHealth);
    }
}
