using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Carlos Coronel

public class RangeZombie : Zombie
{
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] float _runSpeed;
    [SerializeField] float _runRange;
    [SerializeField] float _runTime;
    [SerializeField] float _bulletSpeed;

    private bool _isRunning;

    protected override void AttackAndMove()
    {
        if (_isRunning) return;
        if (distanceToPlayer <= _runRange)
        {
            //escapa por unos segundos
            print("Deber�a correr");
            if (!_isRunning)
            {
                print("Llamo a corrutina");
                StartCoroutine(RunAwayFromPlayer());
            }
            _canMove = true;
        }
        else if (distanceToPlayer <= _attackRange)
        {
            StopAllCoroutines();
            //Le dispara
            if (_actualCooldown <= 0)
            {
                Bullet newBullet = Instantiate(_bulletPrefab, _attackPoint.position, Quaternion.identity);
                newBullet.InitializeBullet(_attackDamage, 4f, _bulletSpeed);
                newBullet.transform.forward = _playerDir;

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

            StopAllCoroutines();
            _movement.RestartSpeed();
        }

        transform.LookAt(_playerTransform.position);
    }

    IEnumerator RunAwayFromPlayer()
    {
        print("Entr� a la corrutina");
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
