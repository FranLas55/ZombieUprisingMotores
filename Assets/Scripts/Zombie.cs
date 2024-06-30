using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Entity
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1.5f; 
    [SerializeField] private float _nextAttackTime;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private int _attackDamage = 1; 

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();
        if (_playerTransform != null)
        {
            Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;

            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= _attackRange)
            {
                if (Time.time >= _nextAttackTime)
                {
                    AttackPlayer();
                    _nextAttackTime = Time.time + 1f / _attackRate; 
                }

            }
            else
            {
                transform.Translate(directionToPlayer * _moveSpeed * Time.deltaTime, Space.World);

                transform.LookAt(_playerTransform);
            }
        }
    }

    private void AttackPlayer()
    {
        if (_playerTransform != null)
        {
            Player player = _playerTransform.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(_attackDamage);
            }
        }
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}



