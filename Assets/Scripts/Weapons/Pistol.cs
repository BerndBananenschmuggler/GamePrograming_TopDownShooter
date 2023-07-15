using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
}
public class Pistol : RangeWeapon
{

    
    // Könnte eigentlich in der BaseClass bleiben
    public override void Fire()
    {
        // Spawn Bullet
        if (_bulletPrefab != null)
            if (_bulletSpawnPointTransform != null)
                Instantiate(_bulletPrefab, _bulletSpawnPointTransform.position, Quaternion.LookRotation(transform.forward));
    }

    public override void Reload()
    {
        throw new NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 startPos = transform.position + transform.forward;
        Gizmos.DrawRay(startPos, transform.forward);
    }
}
