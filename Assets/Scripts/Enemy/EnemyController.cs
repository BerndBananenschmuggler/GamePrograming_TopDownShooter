using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public enum States { Move, Attack }
    public States State;

    public LayerMask HurtboxLayermask;

    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private RangeWeapon _equippedWeapon;
    [SerializeField] private NavMeshAgent _navAgent;

    private const float MAX_ATTACK_ANGLE = 0.5f;

    private Transform _playerTransform;
    private float _distanceToPlayer = float.MaxValue;
    private LayerMask _playerSightScanIgnoreLayermask;

    private void Awake()
    {
        _playerSightScanIgnoreLayermask = LayerMask.NameToLayer("Hurtbox");
    }

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
        // Check if looking at player?
        // Maybe turn towards player when firing dont care while moving
        //if(|| Vector3.Angle(transform.position, _playerTransform.position) > MAX_ATTACK_ANGLE)

        Debug.Log($"Is Player visible: {IsPlayerVisibleFromPosition(transform.position)}");

        if (_distanceToPlayer > _attackRange || IsPlayerVisibleFromPosition(transform.position) == false)
        {
            Move();
            State = States.Move;
        }
        else
        {
            StopMove();
            Attack();
            State = States.Attack;
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
        _navAgent.SetDestination(transform.position);
    }

    private void Attack()
    {
        // Aim Phase
        // - Check if ray between transform and playerTransform hits anything but the player -> Player in sight,
        // - if not in sight -> Move further until in sight

        // Fire Phase
        // - Use the weapon once

        // Repeat until State is not attack
    }

    private bool IsPlayerVisibleFromPosition(Vector3 position)
    {
        RaycastHit hit;
        //LayerMask ignoreMask = LayerMask.NameToLayer("Hurtbox");
        // ~(1 << _playerSightScanIgnoreLayermask) -> Reverse of the Layer that should be ignored is accepted as Raycast targets
        Physics.Raycast(position, _playerTransform.position - position, out hit, float.MaxValue, ~(1 << _playerSightScanIgnoreLayermask));
        Debug.Log($"Hit layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        //Debug.Log($"Hit name: {hit.collider.name}");

        return hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }
}
