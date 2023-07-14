using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 300f;

    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

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
        }
    }
}
