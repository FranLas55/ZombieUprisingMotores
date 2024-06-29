using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    public override void Shoot()
    {
        if (_actualBullets > 0)
        {
            Vector3 shootDirection  = _shootPoint.forward;

            Bullet newBullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);

            newBullet.transform.forward = shootDirection;

            newBullet.InitializeBullet(_damage, _bulletLifeTime, _bulletSpeed);

            _actualBullets--;
        }
    }

}
