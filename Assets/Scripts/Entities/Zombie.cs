using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

//Carlos Coronel

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Entity
{
    //[SerializeField] private float _moveSpeed = 2f;

    [SerializeField] protected Transform _playerTransform;

    [SerializeField] protected bool _canMove;

    protected Vector3 _playerDir = new();
    protected bool _isDeath;

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
    [SerializeField] protected string _isDeathName = "isDeath";
    [SerializeField] protected string _isWalkingName = "isWalking";
    [SerializeField] protected string _onAttackName = "onAttack";

    [Header("Audio")]
    [SerializeField] private AudioClip _growlClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _dieClip;

    protected float _actualCooldown;
    private Ray _attackRay = new();
    private RaycastHit _attackHit;

    protected NavMeshAgent _navAgent;

    protected override void Start()
    {
        base.Start();
        _source = GetComponent<AudioSource>();
        //_playerTransform = Player.Instance.transform;
        _actualCooldown = _attackCooldown;
        _animator.SetBool(_isWalkingName, true);

        _navAgent = GetComponent<NavMeshAgent>();

        _navAgent.speed = _speed;
    }

    protected virtual void Update()
    {
        if (_canMove == _navAgent.isStopped)
        {
            _navAgent.isStopped = !_canMove;
        }

        if (_isDeath) return;
        
        _playerDir = (_playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        AttackAndMove();
        
        if(transform.position.y <= -10) Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        if (_isDeath) return;
        if (_canMove)
        {
            //if (!IsBlocked(_playerDir)) _movement.Move(_playerDir);
            _navAgent.SetDestination(_playerTransform.position);
        }
    }

    protected virtual void AttackAndMove()
    {
        if (distanceToPlayer <= _attackRange)
        {
            if (_actualCooldown <= 0)
            {
                _animator.SetBool(_isWalkingName, false);
                _animator.SetTrigger(_onAttackName);

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
            //transform.LookAt(_playerTransform.position);
        }
    }

    public virtual void Attack()
    {
        _attackRay = new Ray(_attackPoint.position, _playerDir);

        if (Physics.Raycast(_attackRay, out _attackHit, _attackRange, _attackMask))
        {
            if (_attackHit.collider.TryGetComponent(out Player player))
            {
                player.TakeDamage(_attackDamage);
            }
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
        GetComponent<Collider>().enabled = false;
        _rb.useGravity = false;
        _canMove = false;
        _isDeath = true;
        
        _movement.Stop();
        _animator.SetTrigger(_onDeathName);
        GameManager.Instance.AddPoints(_pointsOnDeath);
        Debug.Log("Muere, Animacion");
    }

    public void InitializeZombie(Transform target)
    {
        _playerTransform = target;
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveZombie(this);
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        _blood.Play();
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
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

    public void AnimacionMuerte()
    {
        _animator.SetTrigger(_onDeathName);
    }

   /*IEnumerator DeathZombies()
    {
        _animator.SetTrigger(_onDeathName);
        //_animator.SetBool(_isDeathName, true);
        GameManager.Instance.AddPoints(_pointsOnDeath);
        Debug.Log("Muere, Animacion");

        yield return (0.1f);

        Destroy(gameObject);

    }*/
}