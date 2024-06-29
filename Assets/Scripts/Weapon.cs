using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected string _isShootingName = "isShooting";
    [SerializeField] protected string _cadenceName = "cadence";

    [Header("Values")]
    [SerializeField] protected int _damage;
    [SerializeField] protected float _cadence;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _bulletLifeTime;

     protected int _actualBullets;

    [Header("bullets")]
    [SerializeField] protected float _weaponRange;
    [SerializeField] protected int _maxBullets;
    [SerializeField] protected TrailRenderer _bulletTrail;
    [SerializeField] protected bool _hasVariation;
    [SerializeField] protected Vector3 _bulletVariation = new Vector3(.06f, .06f, .06f);

    private Ray _shootRay;
    private RaycastHit _shootHit;

    private void Start()
    {
        _actualBullets = _maxBullets;

    }

    public virtual void Shoot()
    {
        _shootRay = new Ray(_shootPoint.position, GetDirection()); 

        if (Physics.Raycast(_shootRay, out _shootHit, _weaponRange)) 
        {

            Debug.Log($"Golpee algo, especificamente con {_shootHit.collider.gameObject.name}");

            TrailRenderer trail = Instantiate(_bulletTrail, _shootPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, _shootHit));

            if (_shootHit.collider.TryGetComponent<Zombie>(out var zombie))
            {
                zombie.TakeDamage(_damage);
            }

            _actualBullets--;
          
        }
    }

    public void Recharge()
    {
        _actualBullets = _maxBullets;
      
    }


    private Vector3 GetDirection()
    {
        Vector3 direction = -_shootPoint.right;

        if (_hasVariation)
        {
            direction += new Vector3(
                    Random.Range(-_bulletVariation.x, _bulletVariation.x),
                    Random.Range(-_bulletVariation.y, _bulletVariation.y),
                    Random.Range(-_bulletVariation.z, _bulletVariation.z)
                );

            direction = direction.normalized;
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer localTrail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = localTrail.transform.position;

        while (time < 1)
        {
            localTrail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / localTrail.time;

            yield return null;
        }

        localTrail.transform.position = hit.point;
      

        Destroy(localTrail.gameObject, localTrail.time);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_shootRay);
    }

    public int ReturnBullets()
    {
        return _actualBullets;
    }
}

namespace weapon
{
    public enum WeaponEnum
    {
        Gun,
        ShotGun,
        MachineGun
    }

}
