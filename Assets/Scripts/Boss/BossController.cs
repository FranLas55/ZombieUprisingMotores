//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _attackRange;

    private NavMeshAgent _agent;
    private BossAnimations _animations;

    private bool _hasStarted;
    private bool _isRunning;
    
    private bool _canAttack = true;

    [SerializeField] private float _timeBetweenAttacks;
    private float _actualTime;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _actualTime = _timeBetweenAttacks;
    }

    private void Update()
    {
        if (!_target) return;
        if (!_hasStarted)
        {
            _animations.OnStart();
            _hasStarted = true;
        }

        if (!_canAttack) return;
        
        var distance = Vector3.Distance(transform.position, _target.position);

        _agent.isStopped = distance > _attackRange;
        
        if (_actualTime >= _timeBetweenAttacks)
        {
            AskForAttack(distance);
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
        if (Random.Range(0, 100) % 2 != 0 || !_canAttack) return;
        
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
                    _animations.Grab();
                    break;
                default:
                    print(random);
                    break;
            }
            
            _canAttack = false;
        }
    }

    public void Punch()
    {
        
    }

    public void FloorAttack()
    {
        
    }

    public void Grab()
    {
        
    }

    public void Launch()
    {
        
    }

    public void FinishAttack()
    {
        _canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
