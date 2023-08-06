using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public enum States { Move, Attack }
    public States CurrentState;

    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private RangeWeapon _equippedWeapon;
    [SerializeField] private NavMeshAgent _navAgent;
    
    private Transform _playerTransform;
    private float _distanceToPlayer = float.MaxValue;

    private const float MAX_ATTACK_ANGLE = 0.5f;
    private Coroutine _attackRoutine;
    private float _aimDuration = 1.5f;
    private float _shootLoadupDuration = 0.5f;
    private float _attackCooldownDuration = 1f;
    private bool _aimLocked = false;
    private Vector3 _aimDirection;
    private float _aimStartTime;

    private void Start()
    {
        _playerTransform = PlayerController.Instance.transform;
        if (_playerTransform == null)
            Debug.LogWarning("PlayerTransform is missing");
    }

    // Update is called once per frame
    void Update()
    {
        _distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);

        if (_distanceToPlayer > _attackRange || IsPlayerVisibleFromPosition(transform.position) == false)
        {
            CurrentState = States.Move;
            //Move();
        }
        else
        {
            if(CurrentState == States.Move)
            {
                CurrentState = States.Attack;
                StopMove();
            }

            if (_attackRoutine == null)
            {
                Debug.Log("CoroutineStart");
                _attackRoutine = StartCoroutine(Attack());
            }            
        }
    }    

    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, _attackRange);

        if(Application.isPlaying )
        {
            Handles.color = Color.red;
            Handles.DrawLine(_playerTransform.position, transform.position, 3f);
        }        
    }

    private void Move()
    {
        // Move towards player
        _navAgent.SetDestination(_playerTransform.position);
    }

    private void StopMove()
    {
        // Stay in current position
        _navAgent.ResetPath();
    }   

    private IEnumerator Attack()
    {
        _aimLocked = false;
        _aimStartTime = Time.time;

        int aimLogCounter = 0;

        Debug.Log("Attack Started");

        // Rotate Enemy in direction towards Player (Weapon should always be on enemies forward)
        while (!_aimLocked && Time.time < _aimStartTime + _aimDuration) 
        {
            if (aimLogCounter++ == 0)
                Debug.Log("Aiming...");

            // If State Changed => Abort Attack
            if (CurrentState == States.Move)
            {
                Debug.Log("Attack Aborted");
                _attackRoutine = null;
                yield break;
            }

            _aimDirection = (_playerTransform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(_aimDirection, Vector3.up);
            
            yield return null;
        }        

        // Aim is locked now
        _aimLocked = true;
        Debug.Log("Aim Locked");

        yield return new WaitForSeconds(_shootLoadupDuration);

        Debug.Log("Fire");
        _equippedWeapon.Fire();

        yield return new WaitForSeconds(_attackCooldownDuration);
        _attackRoutine = null;
    }

    private bool IsPlayerVisibleFromPosition(Vector3 position)
    {
        RaycastHit hit;
        //LayerMask ignoreMask = LayerMask.NameToLayer("Hurtbox");
        // ~0 is Reverse of Nothing -> Everything + Ignore Triggers
        Physics.Raycast(position, _playerTransform.position - position, out hit, float.MaxValue, ~0, QueryTriggerInteraction.Ignore);                

        return hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
