using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public abstract class RangeWeapon : MonoBehaviour 
    {
        [SerializeField] protected float _fireRate = 20f;     // 20 shots/sec
        [SerializeField] protected float _reloadTime = 3f;
        [SerializeField] protected float _maxAmmunition = 90;
        [SerializeField] protected float _totalAmmunition = 0;
        [SerializeField] protected float _currentAmunition = 0;
        [SerializeField] protected float _magSize = 30;
        [SerializeField] protected Transform _bulletSpawnPointTransform;
        [SerializeField] protected Bullet _bulletPrefab;
        protected Coroutine _fireRoutine;       // Routine that fires and waits for set seconds to match fireRate
        protected Coroutine _reloadRoutine;     // Routine waits for set reloadTime and refill the _currentAmunition with the remaining ammo  


        private void Awake()
        {
            if (_bulletPrefab == null)
                Debug.LogWarning("BulletPrefab is missing.");
            if(_bulletSpawnPointTransform == null)
                Debug.LogWarning("BulletSpawnPointTransform is missing.");

            _totalAmmunition = _maxAmmunition;
        }

        public abstract void Fire();
        public abstract void Reload();
    }
}
