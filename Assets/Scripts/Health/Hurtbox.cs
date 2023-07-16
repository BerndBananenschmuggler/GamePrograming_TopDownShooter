using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : CustomColliderCreator
{
    [SerializeField] private Health _health;

    public void Trigger(float damage)
    {
        _health.ApplyDamage(damage);
    }
}
