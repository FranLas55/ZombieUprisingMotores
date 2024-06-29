using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using weapon;

//Tobias Rodriguez

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected Player _player;

    [Header("Animations")]
    [SerializeField] protected Animator _animator;
    [SerializeField] private string _onRechargeName = "OnRecharge";
    [SerializeField] protected string _onShootName = "OnShoot";


    [Header("Values")]
    [SerializeField] protected int _damage;
    [SerializeField] protected float _cadence;
    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected float _bulletLifeTime;
    [SerializeField] private WeaponEnum _thisWeapon;


    [Header("bullets")]
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _maxBullets;
    [SerializeField] protected bool _hasVariation;
    [SerializeField] protected Vector3 _bulletVariation = new Vector3(.06f, .06f, .06f);

    protected int _actualBullets;
    private int _actualAmmo;

    private void Start()
    {
        _actualBullets = _maxBullets;
        _actualAmmo = _maxAmmo;
    }

    protected virtual void Update()
    {
        NoBullets();
    }

    public abstract void Shoot();

    public void Recharge()
    {
        if (_actualAmmo > _maxBullets)
        {
            _actualBullets = _maxBullets;
            _actualAmmo -= _maxBullets;
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
        _animator.SetTrigger(_onRechargeName);
        _player.shootAndRecharge = Shoot;
    }

    public int ReturnBullets() //para la UI
    {
        return _actualBullets;
    }

    public WeaponEnum ReturnType()
    {
        return _thisWeapon;
    }

    private void NoBullets()
    {
        if (_actualAmmo <= 0 && _actualBullets <= 0)
        {
            print("no bullets");
            _player.ChangeWeapon(WeaponEnum.Gun);
        }
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
