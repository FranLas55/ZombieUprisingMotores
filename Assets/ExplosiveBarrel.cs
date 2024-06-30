using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] Explosion _explosionPrefab;
    [SerializeField] private float _explosionRadius;

    int _life = 3;

    public void OnDeath()
    {
        Explosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.SetRadius(_explosionRadius);
        Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        _life--;
        if (_life <= 0)
        {
            OnDeath();
        }
    }
}
