using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tobias Rodriguez

public class Machinegun : Weapon
{
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private int _maxBulletsPerBurst = 10;


    protected bool _isShooting;

    public override void Shoot()
    {
        if (!_isShooting)
        {
            StartCoroutine(ShootBurst());
        }

        if(_actualBullets <= 0)
        {
            Recharge();
        }
    }

    private IEnumerator ShootBurst()
    {
        _isShooting = true;

        int bulletsFire = 0;

        while (bulletsFire < _maxBulletsPerBurst && _actualBullets > 0)
        {
            Vector3 variation = new Vector3(Random.Range(-_bulletVariation.x, _bulletVariation.x),
                                                Random.Range(-_bulletVariation.y, _bulletVariation.y),
                                                Random.Range(-_bulletVariation.z, _bulletVariation.z));

            Vector3 shootDirection = (Camera.main.transform.forward + variation).normalized;

            //ProyectileBullet newProyectileBullet = Instantiate(proyectileBulletPrefab, _shootPoint.position, Quaternion.identity);
            //newProyectileBullet.transform.forward = shootDirection;
            //newProyectileBullet.InitializeBullet(_damage, _bulletLifeTime, _bulletSpeed);
            ShotRay(Camera.main.transform.position,shootDirection);

            bulletsFire++;
            _actualBullets--;
            _animator.SetTrigger(_onShootName);

            UpdateUI(false);

            yield return new WaitForSeconds(_fireRate);
        }

        _isShooting = false;

        //_isShooting = false;
    }
}
