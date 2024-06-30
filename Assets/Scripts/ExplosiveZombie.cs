using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZombie : Zombie
{
    [SerializeField] private GameObject _explosionPrefab;

    private Transform _playerTransform;
    private bool _isDead = false;
    private bool _hasExploted = false;

    protected override void Start()
    {
        base.Start();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        if (_isDead && !_hasExploted)
        {
            Explode();

            if (_playerTransform != null)
            {
                float distanceToPLayer = Vector3.Distance(transform.position, _playerTransform.position);
                float explosionRadius = _explosionPrefab.GetComponent<Explosion>().GetRadius();
                if (distanceToPLayer <= explosionRadius)
                {
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    _hasExploted =true;
                }
            }
        }
    }
        protected override void OnDeath()
        {
            base.OnDeath();
            _isDead = true;
            _hasExploted = false;

        }

    private void Explode()
    {
        Instantiate(_explosionPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}

