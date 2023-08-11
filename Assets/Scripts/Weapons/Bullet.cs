using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour 
    {
        [SerializeField] float _speed = 25f;
        [SerializeField] float _lifeTime = 10f;

        private Rigidbody _rigidbody;
        private Coroutine _selfDestructionRoutine;
        private RangeWeapon _ownerWeapon;
        private Hitbox _hitbox;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();     
            _hitbox = GetComponentInChildren<Hitbox>();

            if (_hitbox == null)
                Debug.LogWarning("Hitbox is missing");
            _hitbox.HitDetectionSucceeded += HandleHitDetectionSucceeded;
            _hitbox.HitDetectionFailed += HandleHitDetectionFailed;
        }        

        private void Start()
        {
            // Start Self Destruction Coroutine
            _selfDestructionRoutine = StartCoroutine(DestroySelf());
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }

        public void SetOwner(RangeWeapon owner)
        {
            _ownerWeapon = owner;
        }

        private IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(_lifeTime);

            Destroy(gameObject);
        }

        private void HandleHitDetectionSucceeded(GameObject colliderObject)
        {
            if (colliderObject == null)
                return;

            // Ignore friendly colliders
            // Hurtbox -> Character.layer
            if (colliderObject.transform.parent.gameObject.layer == _ownerWeapon.Owner.layer ||
                colliderObject.layer == _ownerWeapon.Owner.layer)
            {
                //HandleFriendlyFire(colliderObject);
                Debug.Log("HitFail");
                return;
            }

            Debug.Log("HitSuccess");

            colliderObject.GetComponent<Hurtbox>().Trigger(_ownerWeapon.Damage);

            Destroy(gameObject);
        }

        private void HandleHitDetectionFailed(GameObject colliderObject)
        {
            if (colliderObject == null)
                return;
            
            Debug.Log("HitFail");

            //Debug.LogWarning($"if ({colliderObject.transform.parent.gameObject.layer} == " +
            //                 $"{_ownerWeapon.Owner.layer} || {colliderObject.layer} == {_ownerWeapon.Owner.layer})");

            // Ignore friendly colliders
            if (colliderObject.transform.parent.gameObject.layer == _ownerWeapon.Owner.layer ||
                colliderObject.layer == _ownerWeapon.Owner.layer) 
            {
                //HandleFriendlyFire(colliderObject);
                return;
            }

            Destroy(gameObject);
        }

        private void HandleFriendlyFire(GameObject colliderObject)
        {
            Debug.LogError($"KOLLEGE ABGESCHOSSEN - Own Layer: {LayerMask.LayerToName(_ownerWeapon.Owner.layer)} " +
                               $"Hit Layer: {LayerMask.LayerToName(colliderObject.transform.parent.gameObject.layer)}");
            // Reactivate hitbox after unallowed hit
            //_hitbox.Activate();
            Debug.LogWarning($"IsActive: {_hitbox.gameObject.activeSelf}");
        }
    }
}
