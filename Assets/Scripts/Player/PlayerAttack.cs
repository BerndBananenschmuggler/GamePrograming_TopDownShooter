using Assets.Scripts.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public event Action Attacked;
        public event Action Reloaded;
        public event Action ReloadStarted;

        [SerializeField] private LayerMask _groundLayermask;

        [SerializeField] private RangeWeapon _equippedWeapon;     // IST NULL wenn nicht zugewiesen
        [SerializeField] private Transform _weaponPositionTransform;
        private Vector3 _weaponPositionStart;
        private float _weaponPositionDistance;

        private Vector3 _mouseWorldPosition = Vector3.zero;

        private PlayerMovement _playerMovement;


        private void Awake()
        {
            if (_weaponPositionTransform == null)
                Debug.LogWarning("WeaponPosition is missing.");

            // Save WeaponPosition before changing it the first time
            _weaponPositionStart = _weaponPositionTransform.position;
            // Get Distance Player-To-WeaponPositionStart
            _weaponPositionDistance = Mathf.Abs(Vector3.Distance(transform.position, _weaponPositionStart));
        }

        private void Start()
        {
            GameManager.Instance.WaveCleared += HandleWaveCleared;
        }

        private void HandleWaveCleared()
        {
            if (_equippedWeapon == null)
                return;

            _equippedWeapon.RefillAmmo();
        }

        private void OnEnable()
        {
            _equippedWeapon.ReloadCompleted += HandleWeaponReloaded;
            _equippedWeapon.ReloadStarted += HandleWeaponReloadStarted;

            _playerMovement = GetComponent<PlayerMovement>();
            _playerMovement.OnMoved += HandlePlayerMoved;
        }

        

        private void OnDisable()
        {
            _equippedWeapon.ReloadCompleted -= HandleWeaponReloaded;
        }

        private void Update()
        {
            // Set WeaponPosition in Direction Player-To-Mouse
            _weaponPositionTransform.position = GetWeaponPosition(_mouseWorldPosition, transform.position, _weaponPositionDistance);

            // Rotate the WeaponPosition towards the Direction Player-To-Mouse            
            Quaternion newRotation = GetWeaponRotation(transform.position, _weaponPositionTransform.position);
            if (_weaponPositionTransform.rotation != newRotation)
                _weaponPositionTransform.rotation = newRotation;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_mouseWorldPosition, .1f);

            Gizmos.DrawWireSphere(_weaponPositionTransform.position, .5f);
        }

        public RangeWeapon GetEquippedWeapon()
        {
            return _equippedWeapon;
        }

        public void HandleLookInput(CallbackContext context)
        {
            UpdateMousePosition();        
        }        

        public void HandleFireInput(CallbackContext context)
        {
            if (_equippedWeapon == null)
                return;

            if (context.phase != UnityEngine.InputSystem.InputActionPhase.Started) 
                return;

            _equippedWeapon.Fire();

            Attacked?.Invoke();
        }

        public void HandleReloadInput(CallbackContext context)
        {
            if (_equippedWeapon == null)
                return;

            if (context.phase != UnityEngine.InputSystem.InputActionPhase.Started)
                return;

            _equippedWeapon.Reload();
        }

        /// <summary>
        /// Returns direction of <paramref name="mousePosition"/> to  <paramref name="playerPosition"/> 
        /// with range <paramref name="distanceWeaponToPlayer"/> in a circle around <paramref name="playerPosition"/>.
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <param name="playerPosition"></param>
        /// <param name="distanceWeaponToPlayer"></param>
        /// <returns></returns>
        private Vector3 GetWeaponPosition(Vector3 mousePosition, Vector3 playerPosition, float distanceWeaponToPlayer)
        {
            return playerPosition + ((mousePosition - playerPosition).normalized * distanceWeaponToPlayer);        
        }

        /// <summary>
        /// Returns a Rotation looking from <paramref name="playerPosition"/> to <paramref name="weaponPosition"/>.
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <param name="weaponPosition"></param>
        /// <returns></returns>
        private Quaternion GetWeaponRotation(Vector3 playerPosition, Vector3 weaponPosition)
        {
            // Check if the positions are the same (within a small tolerance).
            if (Vector3.Distance(playerPosition, weaponPosition) < 0.001f)
            {
                // Return the current rotation of the weapon (or any default rotation).
                return _weaponPositionTransform.rotation;
            }
            else
            {
                // Calculate the rotation needed for the weapon to face the player.
                return Quaternion.LookRotation(weaponPosition - playerPosition, Vector3.up);
            }
        }

        private void UpdateMousePosition()
        {
            // Rotate weaponSpawnPoint around Player in direction Player-To-Mouse
            Vector3 mousePosition;
            try
            {
                // Get MousePosition
                mousePosition = Utils.GetMousePosition(Camera.main, _groundLayermask, transform);

                _mouseWorldPosition = mousePosition;
            }
            catch { }
        }

        private void HandlePlayerMoved(Vector3 obj)
        {
            UpdateMousePosition();
        }

        private void HandleWeaponReloaded()
        {
            Reloaded?.Invoke();
        }

        private void HandleWeaponReloadStarted()
        {
            ReloadStarted?.Invoke();
        }
    }
}
