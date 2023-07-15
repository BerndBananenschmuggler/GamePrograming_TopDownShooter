using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Player
{
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
    
        private void FixedUpdate()
        {
            // Trigger the OnMoved event if moveDir != 0
            if (_moveDirection != Vector3.zero)
            {            
                // Invokes event and triggers rotationTarget change
                InvokeOnMoved();
            }

            // Set the velocity
            _rigidbody.velocity =
                (
                    Vector3.right * _moveDirection.x +
                    Vector3.up * _rigidbody.velocity.y +
                    Vector3.forward * _moveDirection.z
                ) * _moveSpeed;
        }

        public void HandleMovementInput(CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            _moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        }

        protected virtual void InvokeOnMoved()
        {
            OnMoved?.Invoke(_moveDirection);

            RotateTowards(transform.position + _moveDirection);
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
}

