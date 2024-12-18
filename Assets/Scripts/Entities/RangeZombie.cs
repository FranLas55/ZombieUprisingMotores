using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

//Carlos Coronel

public class RangeZombie : Zombie
{
    [FormerlySerializedAs("_bulletPrefab")] [SerializeField]
    ProyectileBullet proyectileBulletPrefab;

    [SerializeField] float _runSpeed;
    [SerializeField] float _runRange;
    [SerializeField] float _runTime;
    [SerializeField] float _bulletSpeed;

    private bool _isRunning;

    protected override void FixedUpdate()
    {
        if(isThrown) return;
        if (_isDeath) return;
        if (_canMove)
        {
            if (_isRunning)
            {
                //if(!IsBlocked(-_playerDir)) _movement.Move(_playerDir);
                _navAgent.SetDestination((-_playerTransform.position + 2 * transform.position));
            }
            else
            {
                base.FixedUpdate();
            }
        }
    }

    protected override void AttackAndMove()
    {
        if (_isDeath)
        {
            StopAllCoroutines();
            return;
        }
        if (_isRunning) return;
        if (distanceToPlayer <= _runRange)
        {
            //escapa por unos segundos
            print("Debería correr");
            if (!_isRunning)
            {
                print("Llamo a corrutina");
                StartCoroutine(RunAwayFromPlayer());
            }

            _canMove = true;
            _animator.SetBool(_isWalkingName, true);
            transform.forward = -_playerDir;
        }
        else if (distanceToPlayer <= _attackRange)
        {
            StopAllCoroutines();
            //Le dispara
            if (_actualCooldown <= 0)
            {
                _animator.SetBool(_isWalkingName, false);
                _animator.SetTrigger(_onAttackName);

                _actualCooldown = _attackCooldown;
            }
            else
            {
                _actualCooldown -= Time.deltaTime;
            }

            transform.forward = _playerDir;

            _canMove = false;
        }
        else
        {
            _canMove = true;
            _animator.SetBool(_isWalkingName, true);

            StopAllCoroutines();
            _movement.RestartSpeed();
            transform.forward = _playerDir;
        }
    }

    public override void Attack()
    {
        ProyectileBullet newProyectileBullet =
            Instantiate(proyectileBulletPrefab, _attackPoint.position, Quaternion.identity);
        newProyectileBullet.InitializeBullet(_attackDamage, 4f, _bulletSpeed);
        newProyectileBullet.transform.forward = _playerDir;
    }

    IEnumerator RunAwayFromPlayer()
    {
        print("Entró a la corrutina");
        _isRunning = true;
        float t = 0;
        _movement.ChangeSpeed(_runSpeed);

        while (t < _runTime)
        {
            transform.forward = transform.position - _playerDir;
            t += Time.deltaTime;
            yield return null;
        }

        _isRunning = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _runRange);
        Gizmos.DrawRay(transform.position + transform.up, _playerDir * _runRange);
    }
}