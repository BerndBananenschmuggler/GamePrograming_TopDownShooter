using Assets.Scripts.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPositionTransform;
        [SerializeField] private LayerMask _groundLayermask;

        [SerializeField] private RangeWeapon _equipedWeapon;     // IST NULL wenn nicht zugewiesen
        private Vector3 _weaponPositionStart;
        private float _weaponPositionDistance;

        private Vector3 _mouseWorldPosition = Vector3.zero;
        private void Awake()
        {
            if (_weaponPositionTransform == null)
                Debug.LogWarning("WeaponPosition is missing.");

            // Save WeaponPosition before changing it the first time
            _weaponPositionStart = _weaponPositionTransform.position;
            // Get Distance Player-To-WeaponPositionStart
            _weaponPositionDistance = Mathf.Abs(Vector3.Distance(transform.position, _weaponPositionStart));
        }

        private void Update()
        {
            // Set WeaponPosition in Direction Player-To-Mouse
            _weaponPositionTransform.position = GetWeaponPosition(_mouseWorldPosition, transform.position, _weaponPositionDistance);
            // Rotate the WeaponPosition towards the Direction Player-To-Mouse        
            _weaponPositionTransform.rotation = GetWeaponRotation(transform.position, _weaponPositionTransform.position);
        }        

        public void HandleLookInput(CallbackContext context)
        {
            // Rotate weaponSpawnPoint around Player in direction Player-To-Mouse
            Vector3 mousePosition;
            try
            {
                // Get MousePosition
                mousePosition = Utils.GetMousePosition(Camera.main, _groundLayermask, transform);
            
                _mouseWorldPosition = mousePosition;
            }
            catch{ }        
        }

        public void HandleFireInput(CallbackContext context)
        {
            if (_equipedWeapon == null)
                return;

            if (context.phase != UnityEngine.InputSystem.InputActionPhase.Started) 
                return;

            _equipedWeapon.Fire();
        }

        public void HandleReloadInput(CallbackContext context)
        {
            if (_equipedWeapon == null)
                return;

            if (context.phase != UnityEngine.InputSystem.InputActionPhase.Started)
                return;

            _equipedWeapon.Reload();
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
            return Quaternion.LookRotation(weaponPosition - playerPosition, Vector3.up);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_mouseWorldPosition, .1f);

            Gizmos.DrawWireSphere(_weaponPositionTransform.position, .5f);
        }
    }
}
