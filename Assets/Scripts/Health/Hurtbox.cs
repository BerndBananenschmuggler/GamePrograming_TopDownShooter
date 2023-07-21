using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : CustomColliderCreator
{
    [SerializeField] private Health _health;

    private void Awake()
    {
        if (_health == null)
            _health = GetComponentInParent<Health>();
        if (_health == null)
            Debug.LogWarning("Health is missing");

        _isColliderActive = true;
    }

    public void Trigger(float damage)
    {
        _health.ApplyDamage(damage);
    }
}
