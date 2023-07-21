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
            _hitbox.OnHitDetectionSucceeded += HandleHitDetectionSucceeded;
            _hitbox.OnHitDetectionFailed += HandleHitDetectionFailed;
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

        private void HandleHitDetectionSucceeded(GameObject obj)
        {
            //// Dont do stuff to the Player
            //if (obj.CompareTag("Player"))
            //    return;

            Debug.Log("HitSuccess");

            obj.GetComponent<Hurtbox>().Trigger(_ownerWeapon.Damage);

            Destroy(gameObject);
        }

        private void HandleHitDetectionFailed()
        {
            Debug.Log("HitFail");

            Destroy(gameObject);
        }
    }
}
