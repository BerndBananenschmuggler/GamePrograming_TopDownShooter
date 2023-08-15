using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class Pistol : RangeWeapon
    {
        public AudioClip FireSound;

        public override void Fire()
        {
            if (Time.time < _timeLastShot + _minTimeBetweenShots)
                return;
            if (_currentMagFill <= 0)
                return;
            if (_reloadRoutine != null)
                return;
    
    
            // Call Base FireMethod to reduce currentAmmo and handle auto Reload
            base.Fire();
    
    
            Bullet bullet;
            // Spawn Bullet
            if (_bulletPrefab != null)
                if (_bulletSpawnPointTransform != null)
                {
                    bullet = Instantiate(_bulletPrefab, _bulletSpawnPointTransform.position, Quaternion.LookRotation(transform.forward));
                    bullet.SetOwner(this);

                    PlayBulletSound();
                }
    
            _timeLastShot = Time.time;
        }

        private void PlayBulletSound()
        {
            SoundManager.Instance.PlaySound(FireSound);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 startPos = transform.position + transform.forward;
            Gizmos.DrawRay(startPos, transform.forward);
        }
    }
}