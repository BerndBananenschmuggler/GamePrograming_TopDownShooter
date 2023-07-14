using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public event Action<Vector3> OnMoved;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 300f;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _isRotating = true;

    private Camera _camera;
    private LayerMask _mousePositionLayerMask;
    private float _rotationUpdateThreshold = 0.5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;

        if (_rigidbody == null)
            Debug.LogWarning("Player Rigidbody missing.");
    }

    public void HandleMovementInput(CallbackContext callbackContext)
    {
        Vector2 moveInput = callbackContext.ReadValue<Vector2>();
        _moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector3.zero)
        {
            _rigidbody.velocity =
            (
                Vector3.right * _moveDirection.x +
                Vector3.up * _rigidbody.velocity.y +
                Vector3.forward * _moveDirection.z
            ) * _moveSpeed;

            // Invokes event and triggers rotationTarget change
            OnMoveInvoked();
        }
    }

    protected virtual void OnMoveInvoked()
    {
        OnMoved?.Invoke(_moveDirection);

        RotateTowards(transform.position + _moveDirection);
    }

    // [SOLLTE IN EXTRA CLASS VERSCHOBEN WERDEN]
    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Vector3.zero;

        // Ray von Camera zu MausPosition berechnen.
        // Kollisionspunkt von Ray und Boden = Mausposition auf dem Boden.
        // Keine Kollision mit Objekten, deren Layer nicht Teil der _layerMask sind
        // => Wände, Gegner blocken Ray nicht.
        Ray cameraToGroundRay = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraToGroundRay, out RaycastHit raycastHit, float.MaxValue, _mousePositionLayerMask))
        {
            // Wo trifft Ray von Maus zu Boden -> MausPosition
            mousePosition = raycastHit.point;
            // Gleiche Hoehe Spieler und Ziel -> Schaut geradeaus
            mousePosition.y = transform.position.y;
        }

        return mousePosition;
    }

    /// <summary>
    /// Rotate in direction <paramref name="target"/>.
    /// </summary>
    /// <param name="target"></param>
    private void RotateTowards(Vector3 target)
    {
        // Keine Rotation wenn Spieler dashed
        if (_isRotating == false)
            return;

        // Richtung transform zu target.
        Vector3 direction = target - transform.position;

        // Winkel zw. Richtungsvekt.
        float angle = Vector3.Angle(transform.forward, direction);
        // Blick Richtung, Vorn ist Richtung transform zu target, Up ist um 45  gedreht.
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Wenn Winkel < Mindestunterschied -> Rotation direkt setzen
        // Wenn Winkel > Mindestunterschied -> Smooth Rotation
        if (angle < _rotationUpdateThreshold)
        {
            // Rotation direkt setzen
            _rigidbody.rotation = targetRotation;
            return;
        }
        else
        {
            // Fluessige Rotation von currentRotation zu targetRotation
            _rigidbody.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                                                  _rotationSpeed * Time.fixedDeltaTime / angle);
            _rigidbody.rotation = new Quaternion(0, _rigidbody.rotation.y, 0, _rigidbody.rotation.w);
        }
    }
}
