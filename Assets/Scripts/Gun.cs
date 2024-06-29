using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    public override void Shoot()
    {
        base.Shoot();
        if (_actualBullets > 0)
        {
            Bullet newBullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);

            newBullet.InitializeBullet(_damage, _bulletLifeTime);

            Vector3 shootDirection  = _shootPoint.forward;

            newBullet.Shot(shootDirection.normalized);
            _actualBullets--;
        }
    }

}
