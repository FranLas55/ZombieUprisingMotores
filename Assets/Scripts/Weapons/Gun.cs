using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Tobias Rodriguez

public class Gun : Weapon
{
    public override void Shoot()
    {
        if (_actualBullets > 0)
        {
            Vector3 shootDirection  = Camera.main.transform.forward;

            //newProyectileBullet.transform.forward = shootDirection;
            //newProyectileBullet.InitializeBullet(_damage, _bulletLifeTime, _bulletSpeed);
            ShotRay(Camera.main.transform.position, shootDirection);

            _actualBullets--;
            _animator.SetTrigger(_onShootName);
            UpdateUI(true);
        }
        else
        {
            Recharge();
        }
    }

}
