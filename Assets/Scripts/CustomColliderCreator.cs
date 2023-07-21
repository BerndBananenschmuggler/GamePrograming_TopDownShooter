using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class CustomColliderCreator : MonoBehaviour
{
    public enum ColliderShapes { Box, Sphere, Capsule }

    [Header("Custom Collider Settings")]
    [Tooltip("Collider Shape")]
    [SerializeField]
    private ColliderShapes _colliderShape = ColliderShapes.Box;

    [Tooltip("Collider Size")]
    [SerializeField]
    private Vector3 _colliderSize = Vector3.one;

    [Tooltip("Collider Radius")]
    [SerializeField]
    [Range(0.1f, 50f)]
    private float _colliderRadius = 0.5f;

    [Tooltip("Collider Height")]
    [SerializeField]
    private float _colliderHeight = 1f;

    [Tooltip("Collider Offset")]
    [SerializeField]
    private Vector3 _colliderOffset = Vector3.zero;

    [Tooltip("Collider Active")]
    [SerializeField]
    protected bool _isColliderActive = false;

    private Collider _activeCollider;
    private ColliderShapes _prevColliderShape;
    private Vector3 _prevColliderSize = Vector3.zero;
    private float _prevColliderRadius = -1f;
    private float _prevColliderHeight = -1f;
    private Vector3 _prevColliderOffset;


    private void Start()
    {
        // Create new Collider based on selected ColliderShape
        CreateCollider(_colliderSize, _colliderRadius, _colliderHeight, _colliderOffset);

        // Set previous values to prevent recalculation on first Update-Call
        _prevColliderSize = _colliderSize;
        _prevColliderRadius = _colliderRadius;
        _prevColliderHeight = _colliderHeight;
        _prevColliderOffset = _colliderOffset;
        _prevColliderShape = _colliderShape;
    }

    private void Update()
    {
        // Update Shape, Radius, Offset from Settings
        if (_prevColliderShape != _colliderShape)
        {
            _prevColliderShape = _colliderShape;
            UpdateColliderShape(_colliderShape);
        }
        if (_prevColliderRadius != _colliderRadius || _prevColliderHeight != _colliderHeight || _prevColliderSize != _colliderSize)
        {
            _prevColliderSize = _colliderSize;
            _prevColliderRadius = _colliderRadius;
            _prevColliderHeight = _colliderHeight;
            UpdateColliderRadius(_colliderSize, _colliderRadius, _colliderHeight, _colliderOffset);
        }
        if (_prevColliderOffset != _colliderOffset)
        {
            _prevColliderOffset = _colliderOffset;
            UpdateColliderOffset(_colliderOffset);
        }

        // Update collider active
        if (_activeCollider != null)
            if (_isColliderActive != _activeCollider.enabled)
                _activeCollider.enabled = _isColliderActive;
    }

    public void EnableTrigger()
    {
        _isColliderActive = true;
    }

    public void DisableTrigger()
    {
        _isColliderActive = false;
    }

    private void CreateCollider(Vector3 colliderSize, float colliderRadius, float colliderHeight, Vector3 colliderOffset)
    {
        // Create new Collider associated with ColliderShape
        switch (_colliderShape)
        {
            case ColliderShapes.Box:
                _activeCollider = gameObject.AddComponent<BoxCollider>();
                (_activeCollider as BoxCollider).size = colliderSize;
                (_activeCollider as BoxCollider).center = colliderOffset;
                break;
            case ColliderShapes.Sphere:
                _activeCollider = gameObject.AddComponent<SphereCollider>();
                (_activeCollider as SphereCollider).radius = colliderRadius;
                (_activeCollider as SphereCollider).center = colliderOffset;
                break;
            case ColliderShapes.Capsule:
                _activeCollider = gameObject.AddComponent<CapsuleCollider>();
                (_activeCollider as CapsuleCollider).radius = colliderRadius;
                (_activeCollider as CapsuleCollider).height = colliderHeight;
                (_activeCollider as CapsuleCollider).center = colliderOffset;
                (_activeCollider as CapsuleCollider).direction = 2;
                break;
            default:
                //_activeCollider = null;
                break;
        }

        // Enable Trigger (Disable Physics Collision)
        if (_activeCollider != null)
        {
            _activeCollider.isTrigger = true;
        }
    }

    private void UpdateColliderShape(ColliderShapes colliderShape)
    {
        // Delete Previous Collider
        foreach (var component in gameObject.GetComponents(typeof(Collider)))
        {
            Destroy(component);
        }

        switch (colliderShape)
        {
            case ColliderShapes.Box:
                _activeCollider = gameObject.AddComponent<BoxCollider>();
                break;
            case ColliderShapes.Sphere:
                _activeCollider = gameObject.AddComponent<SphereCollider>();
                break;
            case ColliderShapes.Capsule:
                _activeCollider = gameObject.AddComponent<CapsuleCollider>();
                break;
            default:
                _activeCollider = null;
                break;
        }

        if (_activeCollider != null)
        {
            _activeCollider.isTrigger = true;
        }

        UpdateColliderRadius(_colliderSize, _colliderRadius, _colliderHeight, _colliderOffset);
        UpdateColliderOffset(_colliderOffset);
    }

    private void UpdateColliderRadius(Vector3 colliderSize, float colliderRadius, float colliderHeight, Vector3 colliderOffset)
    {
        if (_activeCollider == null)
            return;

        if (_activeCollider.GetType() == typeof(BoxCollider))
        {
            (_activeCollider as BoxCollider).size = colliderSize;
            (_activeCollider as BoxCollider).center = colliderOffset;
        }
        else if (_activeCollider.GetType() == typeof(SphereCollider))
        {
            (_activeCollider as SphereCollider).radius = colliderRadius;
            (_activeCollider as SphereCollider).center = colliderOffset;
        }
        else if (_activeCollider.GetType() == typeof(CapsuleCollider))
        {
            (_activeCollider as CapsuleCollider).radius = colliderRadius;
            (_activeCollider as CapsuleCollider).height = colliderHeight;
            (_activeCollider as CapsuleCollider).center = colliderOffset;
            (_activeCollider as CapsuleCollider).direction = 2;
        }
    }

    private void UpdateColliderOffset(Vector3 offset)
    {
        if (_activeCollider == null)
            return;

        if (_activeCollider.GetType() == typeof(BoxCollider))
            (_activeCollider as BoxCollider).center = offset;
        else if (_activeCollider.GetType() == typeof(SphereCollider))
            (_activeCollider as SphereCollider).center = offset;
        else if (_activeCollider.GetType() == typeof(CapsuleCollider))
            (_activeCollider as CapsuleCollider).center = offset;
    }
}
