using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : CustomColliderCreator
{
    public event Action<GameObject> OnHitDetectionSucceeded;
    public event Action<GameObject> OnHitDetectionFailed;

    [SerializeField] private LayerMask _hitableLayermask;

    private void Awake()
    {
        if (_hitableLayermask == 0)
            Debug.LogWarning("No layer selected");

        _isColliderActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision detected: {other.name}");

        if ((_hitableLayermask & (1 << other.gameObject.layer)) != 0)
            OnHitDetectionSucceeded?.Invoke(other.gameObject);
        else            
            OnHitDetectionFailed?.Invoke(other.gameObject);

        // Deactivate the hitbox itself to stop it from receiving another TriggerEnter Event
        //gameObject.SetActive(false);

        //Deactivate();
    }

    public void Activate()
    {
        _isColliderActive = true;
        //Debug.LogWarning("HITBOX ENABLED");
    }

    public void Deactivate()
    {
        _isColliderActive = false;
        //Debug.LogWarning("HITBOX DISABLED");
    }
}
