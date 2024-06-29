using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tobias Rodriguez

public class ShotGun : Weapon
{
    [SerializeField] private int _pelletsPerShot = 5;

    public override void Shoot()
    {
        if (_actualBullets > 0)
        {
            _animator.SetTrigger(_onShootName);

            for (int i = 0; i < _pelletsPerShot; i++)
            {
                Vector3 variation = new Vector3(Random.Range(-_bulletVariation.x, _bulletVariation.x),
                                                Random.Range(-_bulletVariation.y, _bulletVariation.y),
                                                Random.Range(-_bulletVariation.z, _bulletVariation.z));

                Vector3 shootDirection = (_shootPoint.forward + variation).normalized;

                Bullet newBullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);

                newBullet.transform.forward = shootDirection;

                newBullet.InitializeBullet(_damage, _bulletLifeTime, _bulletSpeed);
            }

            _actualBullets--;
        }
        else
        {
            _player.shootAndRecharge = Recharge;
        }
    }
}
