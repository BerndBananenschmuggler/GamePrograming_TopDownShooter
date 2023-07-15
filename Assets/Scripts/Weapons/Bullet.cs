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

        private Rigidbody _rigbody;
        private Coroutine _selfDestructionRoutine;

        private void Awake()
        {
            _rigbody = GetComponent<Rigidbody>();            
        }

        private void Start()
        {
            // Start Self Destruction Coroutine
            _selfDestructionRoutine = StartCoroutine(DestroySelf());
        }

        private void FixedUpdate()
        {
            _rigbody.velocity = transform.forward * _speed;
        }

        private IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(_lifeTime);

            Destroy(gameObject);
        }
    }
}
