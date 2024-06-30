using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Entity
{
    //[SerializeField] private float _moveSpeed = 2f;

    [SerializeField] protected Transform _playerTransform;

    protected bool _canMove;

    protected Vector3 _playerDir = new();

    protected float distanceToPlayer;

    [Header("Values")]
    [SerializeField] private int _pointsOnDeath = 7;
    [SerializeField] protected float _attackRange = 1.5f;
    [SerializeField] protected float _attackCooldown = 2f;
    [SerializeField] protected int _attackDamage = 1;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] protected Transform _attackPoint;

    protected float _actualCooldown;
    private Ray _attackRay = new();
    private RaycastHit _attackHit;

    protected override void Start()
    {
        base.Start();
        //_playerTransform = Player.Instance.transform;
        _actualCooldown = _attackCooldown;
    }

    protected virtual void Update()
    {
        _playerDir = (_playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        AttackAndMove();
    }

    protected virtual void FixedUpdate()
    {
        if (_canMove)
        {
            _movement.Move(_playerDir);
        }
    }

    protected virtual void AttackAndMove()
    {
        if (distanceToPlayer <= _attackRange)
        {
            if (_actualCooldown <= 0)
            {
                _attackRay = new Ray(_attackPoint.position, _playerDir);

                if(Physics.Raycast(_attackRay, out _attackHit ,_attackRange, _attackMask))
                {
                    if(_attackHit.collider.TryGetComponent(out Player player))
                    {
                        player.TakeDamage(_attackDamage);
                    }
                }

                print("Ataco");
                _actualCooldown = _attackCooldown;
            }
            else
            {
                _actualCooldown -= Time.deltaTime;
            }

            _canMove = false;
        }
        else
        {
            _canMove = true;
            transform.LookAt(_playerTransform.position);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_attackRay);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    public override void OnDeath()
    {
        GameManager.Instance.AddPoints(_pointsOnDeath);

        Destroy(gameObject);
    }

    public void InitializeZombie(Transform target)
    {
        _playerTransform = target;
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveZombie(this);
    }
}



