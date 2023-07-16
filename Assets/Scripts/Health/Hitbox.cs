using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : CustomColliderCreator
{
    public event Action<GameObject> OnHitDetectionSucceeded;
    public event Action OnHitDetectionFailed;

    [SerializeField] LayerMask _hitableLayermask;

    private void Awake()
    {
        if (_hitableLayermask == 0)
            Debug.LogWarning("No layer selected");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_hitableLayermask & (1 << other.gameObject.layer)) != 0)
            OnHitDetectionSucceeded?.Invoke(other.gameObject);
        else
            OnHitDetectionFailed?.Invoke();
    }
}
