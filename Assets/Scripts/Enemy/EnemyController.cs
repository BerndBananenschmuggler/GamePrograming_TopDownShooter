using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum States { Move, Attack }
    public States State;


    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private RangeWeapon _equippedWeapon;
    [SerializeField] private NavMeshAgent _navAgent;

    private const float MAX_ATTACK_ANGLE = 0.5f;

    private Transform _playerTransform;
    private float _distanceToPlayer = float.MaxValue;

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

        if (_distanceToPlayer > _attackRange)
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

    }
}
