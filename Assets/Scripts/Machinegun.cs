using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machinegun : Weapon
{
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _maxBulletsPerBurst = 10;

    private bool _isShooting;

    private void Update()
    {
        if (_isShooting) StartCoroutine(ShootBurst());
    }
    public override void Shoot()
    {
        _isShooting = true;
    }

    private IEnumerator ShootBurst()
    {
        int bulletsFire = 0;

        while (bulletsFire < _maxBulletsPerBurst && _actualBullets > 0)
        {
            Bullet newBullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);

            newBullet.InitializeBullet(_damage, _bulletLifeTime);

            Vector3 shootDirection = _shootPoint.forward;

            newBullet.Shot(shootDirection.normalized);

            bulletsFire++;

            yield return new WaitForSeconds(_fireRate);
        }

        _isShooting = false;
    }
}
