using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class BossController : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _target;
    [SerializeField] private ParticleSystem _floorAttackSystem;
    
    [Header("Stats")]
    [SerializeField, Range(0, 1)] private float _lifeTest;
    [SerializeField] private int _maxHP;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _distanceAttackRange;
    [SerializeField] private BossStats _baseStats;
    [SerializeField] private BossStats _secondPhaseStats;
    
    [Header("AttackOrigins")]
    [SerializeField] private Transform _punchOrigin;
    [SerializeField] private Transform _grabOrigin;
    [SerializeField] private Transform _grabPosition;

    [Header("Masks")]
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _zombieMask;
    
    private int _actualHp;
    private BossStats _actualStats;

    private NavMeshAgent _agent;
    private BossAnimations _animations;

    private bool _hasStarted;
    private bool _isRunning;
    private bool _isStopped;

    public bool a;
    
    private bool _canAttack = true;
    private float _actualTime;

    private Transform grabTarget;
    
    private Transform _actualPos;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animations = GetComponent<BossAnimations>();
        _actualHp = _maxHP;

        _actualStats = _baseStats;
        
        _agent.speed = _actualStats.speed;
        _actualTime = _actualStats.timeBetweenAttacks;
    }

    private void Update()
    {
        a = _agent.isStopped;
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(10);
        }
        
        if (!_target) return;
        if (!_hasStarted)
        {
            _animations.OnStart();
            _hasStarted = true;
        }
        
        if (grabTarget && _actualPos)
        {
            grabTarget.position = _actualPos.position;
        }

        if (!_canAttack) return;
        
        var distance = Vector3.Distance(transform.position, _target.position);

        _agent.isStopped = distance <= _attackRange;
        
        _animations.SetMove(!_agent.isStopped);

        if (_agent.isStopped)
        {
            if (_isStopped == false)
            {
                _animations.OnStop();
                _isStopped = true;
            }
        }
        else
        {
            _isStopped = false;
        }
        
        if (_actualTime >= _actualStats.timeBetweenAttacks)
        {
            AskForAttack(distance);
            _actualTime = 0;
        }
        else
        {
            _actualTime += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_target) return;
        _agent.SetDestination(_target.position);
    }

    private void AskForAttack(float distance)
    {
        if (!_canAttack) return;
        
        if (distance <= _attackRange)
        {
            var random = Random.Range(0, _isRunning ? 3 : 2);

            switch (random)
            {
                case 0:
                    _animations.Punch();
                    break;
                case 1:
                    _animations.FloorAttack();
                    break;
                case 2:
                    var possiblePlayer = Physics.OverlapSphere(transform.position, _attackRange, _playerMask);
                    
                    var dir = possiblePlayer[0].transform.position - transform.position;
                    
                    if (!Physics.Raycast(transform.position, dir, _attackRange, _obstacleMask))
                    {
                        grabTarget = possiblePlayer[0].transform;
                        _animations.Grab();
                    }
                    break;
                default:
                    print(random);
                    break;
            }
            
            _canAttack = false;
            return;
        }

        var possibleGrab = Physics.OverlapSphere(transform.position, _attackRange, _zombieMask);

        if (possibleGrab.Length > 0)
        {
            var dir = possibleGrab[0].transform.position - transform.position;
            if (!Physics.Raycast(transform.position, dir, _attackRange, _obstacleMask))
            {
                grabTarget = possibleGrab[0].transform;
                _animations.Grab();
            }
        }
    }

    public void Punch()
    {
        _agent.enabled = false;
        _agent.isStopped = true;
        var attackRay = new Ray(_punchOrigin.position, _target.position - _punchOrigin.position);

        if (!Physics.Raycast(attackRay, _attackRange, _obstacleMask))
        {
            if (Physics.Raycast(attackRay, out RaycastHit hitInfo, _attackRange, _playerMask))
            {
                print($"Le pegué una ñapi a {hitInfo.collider.name} por gil");
                
                if (hitInfo.collider.TryGetComponent(out Player player))
                {
                    //player.TakeDamage(_actualStats.damage);
                }
            }
        }
    }

    public void FloorAttack()
    {
        _agent.enabled = false;
        _agent.isStopped = true;
        _floorAttackSystem.Play();
        
        var possibleTarget = Physics.OverlapSphere(transform.position, _attackRange, _playerMask + _zombieMask);

        foreach (var entity in possibleTarget)
        {
            if (entity.TryGetComponent(out Entity paps))
            {
                paps.GetForce(entity.transform.position - transform.position, _actualStats.force);
                //paps.TakeDamage(_actualStats.damage / 2);
            }
        }
    }

    public void Grab()
    {
        _agent.enabled = false;
        _agent.isStopped = true;
        if(!grabTarget) return;
        
        print($"agarre a {grabTarget.name}");

        _actualPos = _grabOrigin;
    }

    public void ChangePos()
    {
        if(!grabTarget) return;
        
        print($"cambie de posicion a {grabTarget.name}");
        
        _actualPos = _grabPosition;
    }

    public void Launch()
    {
        if(!grabTarget) return;
        
        if (grabTarget.TryGetComponent(out Player player))
        {
            player.GetForce(transform.forward, _actualStats.force * 2);

            player.isThrown = true;
            //player.TakeDamage(3);
        }
        else if (grabTarget.TryGetComponent(out Zombie zombie))
        {
            zombie.GetForce(_target.position - transform.position, _actualStats.force);
            if (zombie.TryGetComponent(out NavMeshAgent a))
            {
                a.enabled = false;
            }
            zombie.isThrown = true;
        }

        grabTarget = null;
        _actualPos = null;
    }

    public void FinishAttack()
    {
        _agent.enabled = true;
        _canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _distanceAttackRange);
    }

    private bool _isSecondPhase;
    
    public void TakeDamage(int dmg)
    {
        _actualHp -= dmg;

        if (_actualHp <= _maxHP / 2 && !_isSecondPhase)
        {
            _actualStats = _secondPhaseStats;
            _isRunning = true;
            _animations.Run();
            _isSecondPhase = true;
            _agent.speed = _actualStats.speed;
        }
        else if (_actualHp <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        _animations.OnDeath();
    }
}


[Serializable]
public struct BossStats
{
    public int damage;
    public float speed;
    public float timeBetweenAttacks;
    public float force;
}