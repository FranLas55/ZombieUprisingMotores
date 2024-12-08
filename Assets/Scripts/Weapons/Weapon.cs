using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using weapon;

//Tobias Rodriguez

public abstract class Weapon : MonoBehaviour
{
    [FormerlySerializedAs("_bulletPrefab")]
    [Header("References")]
    //[SerializeField] protected ProyectileBullet proyectileBulletPrefab;
    [SerializeField] protected TrailRenderer _bulletTrail;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected Player _player;

    [Header("Animations")]
    [SerializeField] protected Animator _animator;
    [SerializeField] private string _onRechargeName = "OnRecharge";
    [SerializeField] protected string _onShootName = "OnShoot";
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField, Range(1, 2)] private float _runSpeed;


    [Header("Values")]
    [SerializeField] protected int _damage;
    [SerializeField] private WeaponEnum _thisWeapon;


    [Header("bullets")]
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _maxBullets;
    [SerializeField] protected Vector3 _bulletVariation = new Vector3(.06f, .06f, .06f);

    protected int _actualBullets;
    private int _actualAmmo;

    private Ray _shotRay;
    private RaycastHit _shotHit;

    private void Start()
    {
        _actualBullets = _maxBullets;
        _actualAmmo = _maxAmmo;
    }

    protected virtual void Update()
    {
        NoBullets();
    }

    public void Move(float xAxis, float zAxis)
    {
        _animator.SetFloat(_xAxisName, xAxis);
        _animator.SetFloat(_zAxisName, zAxis);
    }

    public abstract void Shoot();

    public void Recharge()
    {
        if (_actualAmmo > _maxBullets)
        {
            _actualAmmo -= (_maxBullets - _actualBullets);
            _actualBullets = _maxBullets;
        }
        else
        {
            _actualBullets = _actualAmmo;
            _actualAmmo = 0;
        }

        if(_thisWeapon == WeaponEnum.Gun)
        {
            _actualAmmo = _maxAmmo;
        }

        UpdateUI(_thisWeapon == WeaponEnum.Gun);
        _animator.SetTrigger(_onRechargeName);
    }

    public int ReturnBullets() //para la UI
    {
        return _actualBullets;
    }

    public WeaponEnum ReturnType()
    {
        return _thisWeapon;
    }

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _actualBullets = _maxBullets;
        _actualAmmo = _maxAmmo;

        UpdateUI(_thisWeapon == WeaponEnum.Gun);
    }

    protected void UpdateUI(bool infinite)
    {
        GameManager.Instance?.UpdateBullets(_actualBullets, _maxBullets, _actualAmmo, infinite);
    }

    private void NoBullets()
    {
        if (_actualAmmo <= 0 && _actualBullets <= 0)
        {
            print("no bullets");
            _player.ChangeWeapon(WeaponEnum.Gun);
        }
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
        //hacer que se vea donde impactó la bala (a evaluar)

        Destroy(localTrail.gameObject, localTrail.time);
    }

    protected void ShotRay(Vector3 from, Vector3 dir)
    {
        _shotRay = new Ray(from, dir); //Tarea(xd): agregar variacion a la direccion de disparo, para dar más realismo

        if (Physics.Raycast(_shotRay, out _shotHit , Mathf.Infinity)) // no tiene una LayerMask porque quiero que pueda impactar con todo, pero solo hacer daño al zombie
        {
            //Trazar el camino de la bala

            Debug.Log($"Golpee algo, especificamente con {_shotHit.collider.gameObject.name}");

            TrailRenderer trail = Instantiate(_bulletTrail, _shootPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, _shotHit));

            if (_shotHit.collider.TryGetComponent<IDamageable>(out var IDamage))
            {
                IDamage.TakeDamage(_damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var cam = Camera.main;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * 1000);
    }
}

namespace weapon
{
    public enum WeaponEnum
    {
        Gun,
        ShotGun,
        Famas
    }

}
