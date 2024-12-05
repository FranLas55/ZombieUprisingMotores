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

                Vector3 shootDirection = (Camera.main.transform.forward + variation).normalized;

                //ProyectileBullet newProyectileBullet = Instantiate(proyectileBulletPrefab, _shootPoint.position, Quaternion.identity);
                //newProyectileBullet.transform.forward = shootDirection;
                //newProyectileBullet.InitializeBullet(_damage, _bulletLifeTime, _bulletSpeed);
                ShotRay(Camera.main.transform.position,shootDirection);
            }

            _actualBullets--;
            UpdateUI(false);
        }
        else
        {
            Recharge();
        }
    }
}
