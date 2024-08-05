using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carlos Coronel

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [SerializeField] Explosion _explosionPrefab;
    [SerializeField] private float _explosionRadius;

    int _life = 3;

    private void Start()
    {
        Player.Instance.GameOverEvent += Enable;
    }

    public void OnDeath()
    {
        Explosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.SetRadius(_explosionRadius);
        gameObject.SetActive(false);
    }

    private void Enable()
    {
        this.gameObject.SetActive(true);
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
