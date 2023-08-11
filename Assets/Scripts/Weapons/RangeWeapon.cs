using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public abstract class RangeWeapon : MonoBehaviour 
    {
        public event Action ReloadCompleted;

        public GameObject Owner { get { return _owner; } }
        public float Damage { get { return _damage; } }
        public float MaxAmmo { get { return _maxAmmunition; } }
        public float CurrentAmmo { get { return _currentAmunition; } }
        public float MaxMagSize { get { return _maximumMagSize; } }
        public float CurrentMagFill { get { return _currentMagFill; } }

        [SerializeField] protected float _fireRate = 20f;     // 20 shots/sec
        [SerializeField] protected float _reloadTime = 3f;
        [SerializeField] protected float _damage = 10f;
        [SerializeField] protected float _maxAmmunition = 90;
        [SerializeField] protected float _currentAmunition = 0;
        [SerializeField] protected float _maximumMagSize = 30;
        [SerializeField] protected float _currentMagFill = 0;
        [SerializeField] protected Transform _bulletSpawnPointTransform;
        [SerializeField] protected Bullet _bulletPrefab;
        //protected Coroutine _fireRoutine;       // Routine that fires and waits for set seconds to match fireRate
        protected Coroutine _reloadRoutine;       // Routine waits for set reloadTime and refill the _currentAmunition with the remaining ammo  

        protected float _minTimeBetweenShots = 0;
        protected float _timeLastShot = -1;

        protected GameObject _owner;


        private void Awake()
        {
            if (_bulletPrefab == null)
                Debug.LogWarning("BulletPrefab is missing.");
            if(_bulletSpawnPointTransform == null)
                Debug.LogWarning("BulletSpawnPointTransform is missing.");

            
            // Calculate the minimum time between 2 shots while using the fireRate
            _minTimeBetweenShots = 1 / _fireRate;


            // Set currentAmmo to max
            _currentAmunition = _maxAmmunition;
            // Load one full mag
            _currentMagFill = _maximumMagSize;
            // Reduce the total Ammo by one mag
            _currentAmunition -= _maximumMagSize;

            // DEBUG
            //LogAmmo();
        }

        private void Start()
        {
            // Weapon -> WeaponPosition -> Character -> Owner
            _owner = transform.parent.parent.parent.gameObject;
        }

        public virtual void Fire()
        {
            if (_reloadRoutine != null)
                return;

            // Reduce current magFill each shot
            _currentMagFill--;

            // Start Reload when ammo is empty
            if (_currentMagFill <= 0)
                if (_reloadRoutine == null)
                    _reloadRoutine = StartCoroutine(StartReload());

            // DEBUG
            //LogAmmo();
        }
        public void Reload()
        {
            if (_reloadRoutine == null)
                _reloadRoutine = StartCoroutine(StartReload());
        }

        public void StopReload()
        {
            if(_reloadRoutine != null)
            {
                StopCoroutine(_reloadRoutine);
                _reloadRoutine = null;
            }
        }

        private IEnumerator StartReload()
        {
            // Time to start a visual here.
                       
            // Decide if a reload is needed and possible or not
            if (_currentMagFill < _maximumMagSize)
                if (_currentAmunition > 0) 
                {
                    // DEBUG
                    Debug.Log("Start Reloading");

                    // Refill all missing bullets or refill all bullets that are left over if there is less than 1 mag left
                    float bulletsToReload = Mathf.Min(_maximumMagSize - _currentMagFill, _currentAmunition);

                    // Wait for Reload Time
                    yield return new WaitForSeconds(_reloadTime);

                    // Reduce the bullets that are left to reload
                    _currentAmunition -= bulletsToReload;
                    // Refill the mag
                    _currentMagFill += bulletsToReload;
                    
                    // If Something goes Wrong and the mag is overfilled
                    if(_currentMagFill> _maximumMagSize)
                    {
                        // Return the excess ammo in mag to currentAmmo
                        _currentAmunition += _currentMagFill - _maximumMagSize;
                        // Set the MagFill to MagMaxSize
                        _currentMagFill = _maximumMagSize;
                    }

                    // DEBUG
                    Debug.Log("Finished Reloading");

                    // DEBUG
                    //LogAmmo();

                    ReloadCompleted?.Invoke();
                }

            // Make another Reload possible
            _reloadRoutine = null;
        }

        private void LogAmmo()
        {
            Debug.Log($"MaxAmmo: {_maxAmmunition}, CurrentAmmo: {_currentAmunition}");
            Debug.Log($"MaxMagFill: {_maximumMagSize}, CurrentMagFill: {_currentMagFill}");
        }
    }
}
