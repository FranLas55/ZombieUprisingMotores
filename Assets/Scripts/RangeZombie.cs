using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeZombie : Zombie
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _nextAttackTime;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _aggroRange = 10f;

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
           float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            
           if(distanceToPlayer <= _aggroRange)
           {
                if(distanceToPlayer > _attackRange)
                {
                    Vector3 directionPlayer = (_playerTransform.position - transform.position);
                    transform.Translate(directionPlayer * _moveSpeed * Time.deltaTime, Space.World);
                    transform.LookAt(_playerTransform);
                }

                if(Time.time >= _nextAttackTime && distanceToPlayer <= _attackRange)
                {
                    AttackPlayer();
                    _nextAttackTime = Time.time + 1f / _attackRate;
                }
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

