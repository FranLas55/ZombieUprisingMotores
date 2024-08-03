using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carlos Coronel

public class ExplosiveZombie : Zombie
{
    [SerializeField] private Explosion _explosionPrefab;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _explosionRadius;

    private bool _canExplode = false;

    protected override void Update()
    {
        base.Update();

        if (_actualHp <= (float)_hp / 2)
        {
            _canExplode = true;
            _movement.ChangeSpeed(_runSpeed);
        }

        if (_canExplode && distanceToPlayer <= _explosionRadius * .25f)
        {
            Explode();
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Explode();
    }

    private void Explode()
    {
        Explosion myExplosion = Instantiate(_explosionPrefab, transform.position + transform.up, Quaternion.identity);
        myExplosion.SetRadius(_explosionRadius);
        Destroy(gameObject);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + transform.up, _playerDir* _explosionRadius * .25f);
    }

}

