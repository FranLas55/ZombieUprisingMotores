using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

//Carlos Coronel

public class Zombie : Entity
{
    //[SerializeField] private float _moveSpeed = 2f;

    [SerializeField] protected Transform _playerTransform;

    protected bool _canMove;

    protected Vector3 _playerDir = new();

    protected float distanceToPlayer;

    [Header("Values")][SerializeField] private int _pointsOnDeath = 7;
    [SerializeField] protected float _attackRange = 1.5f;
    [SerializeField] protected float _attackCooldown = 2f;
    [SerializeField] protected int _attackDamage = 1;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] protected Transform _attackPoint;

    [Header("Particulas")]
    [SerializeField] protected ParticleSystem _blood;

    [Header("Animations")]
    [SerializeField] protected Animator _animator;
    [SerializeField] protected string _onDeathName = "onDeath";
    [SerializeField] protected string _isWalkingName = "isWalking";
    [SerializeField] protected string _onAttackName = "onAttack";

    [Header("Audio")]
    [SerializeField] private AudioClip _growlClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _dieClip;
    [SerializeField] private AudioClip _explodeClip;

    protected float _actualCooldown;
    private Ray _attackRay = new();
    private RaycastHit _attackHit;

    protected override void Start()
    {
        base.Start();
        _source = GetComponent<AudioSource>();
        //_playerTransform = Player.Instance.transform;
        _actualCooldown = _attackCooldown;
    }

    protected virtual void Update()
    {
        _playerDir = (_playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        AttackAndMove();
        
        if(transform.position.y <= -10) TakeDamage(1000);
    }

    protected virtual void FixedUpdate()
    {
        if (_canMove)
        {
            if (!IsBlocked(_playerDir)) _movement.Move(_playerDir);
        }
    }

    protected virtual void AttackAndMove()
    {
        if (distanceToPlayer <= _attackRange)
        {
            if (_actualCooldown <= 0)
            {
                _attackRay = new Ray(_attackPoint.position, _playerDir);

                if (Physics.Raycast(_attackRay, out _attackHit, _attackRange, _attackMask))
                {
                    if (_attackHit.collider.TryGetComponent(out Player player))
                    {
                        player.TakeDamage(_attackDamage);
                        _animator.SetBool(_isWalkingName, false);
                        _animator.SetTrigger(_onAttackName);
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
            _animator.SetBool(_isWalkingName, true);
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
        _animator.SetTrigger(_onDeathName);
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

    public void BloodAnimation()
    {
        _blood.gameObject.SetActive(true);

    }
    public void PlayGrowlClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }
        _source.clip = _growlClip;
        _source.Play();
    }
    public void PlayAttackClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }
        _source.clip = _attackClip;
        _source.Play();
    }
    public void PlayDeathClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }
        _source.clip = _dieClip;
        _source.Play();
    }
    public void PlayExplodeClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }
        _source.clip = _explodeClip;
        _source.Play();
    }
}