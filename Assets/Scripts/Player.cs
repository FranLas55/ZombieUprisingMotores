using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;
using static GameManager;

//Francisco Lastra

public class Player : Entity
{
    [Header("Values")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private int _shield;
    [SerializeField] private float _jumpForce;
    [SerializeField] private WeaponEnum _weapon = WeaponEnum.Gun;
    [SerializeField] private Weapon[] _myWeapons;

    [Header("Inputs")]
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _rechargeKey = KeyCode.R;

    [Header("JumpRay")]
    [SerializeField] private float _jumpRayRange = .5f;
    [SerializeField] private LayerMask _jumpMask;

    [Header("InteractBox")]
    [SerializeField] private Vector3 _extends;
    [SerializeField] private Transform _center;
    [SerializeField] private LayerMask _buyableMask;

    private Vector3 _dir = new(), _transformOffset = new();

    private Movement _movement;

    private Ray _jumpRay;

    private Weapon _actualWeapon;

    public delegate void Shoot();

    public Shoot shootAndRecharge;

    public delegate bool Buy(int i);
    public event Buy BuyEvent;


    public event VoidDelegate PlayerDead;

    protected override void Start()
    {
        base.Start();
        ChangeWeapon(WeaponEnum.Gun);
        _movement = new Movement(_rb, _speed);
    }

    private void Update()
    {
        _dir = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(_runKey)) _movement.ChangeSpeed(_runSpeed);
        else _movement.RestartSpeed();

        if (IsGrounded() && Input.GetKeyDown(_jumpKey)) Jump();

        if (Input.GetMouseButtonDown(0))
        {
            shootAndRecharge();
        }

        if (Input.GetKeyDown(_rechargeKey)) _actualWeapon.Recharge();

        BuyObj();

        //pruebas
        if (Input.GetKeyDown(KeyCode.M)) ChangeWeapon(WeaponEnum.MachineGun);
        if (Input.GetKeyDown(KeyCode.N)) ChangeWeapon(WeaponEnum.ShotGun);
        if (Input.GetKeyDown(KeyCode.G)) ChangeWeapon(WeaponEnum.Gun);
        if (Input.GetKeyDown(KeyCode.C)) TakeDamage(1);

    }



    private void FixedUpdate()
    {
        _movement.Move(_dir);
    }

    protected override void OnDeath()
    {
        //Puede pasar algo como no
        PlayerDead();
    }

    public void ChangeWeapon(WeaponEnum newWeapon)
    {
        print($"Cambió de arma, la nueva arma es {newWeapon}");
        _weapon = newWeapon;

        foreach (var weapon in _myWeapons)
        {
            //Fijarse si el enum del arma es igual a _weapon y si es el mismo prenderla, si no apagarla
            if (weapon.ReturnType() == _weapon)
            {
                weapon.gameObject.SetActive(true);
                _actualWeapon = weapon;
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }

        shootAndRecharge = _actualWeapon.Shoot;
        _actualWeapon.Initialize();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        float f = (float)_actualHp / _hp;

        GameManager.Instance.UpdateLifeBar(f);
    }

    public override void Heal(int hp)
    {
        base.Heal(hp);

        float f = (float)_actualHp / _hp;

        GameManager.Instance.UpdateLifeBar(f);
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3(transform.position.x,
                                        transform.position.y + _jumpRayRange / 4,
                                        transform.position.z);

        _jumpRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_jumpRay, _jumpRayRange, _jumpMask);
    }

    private void BuyObj()
    {
        Collider[] colliders = Physics.OverlapBox(_center.position, _extends / 2, Quaternion.identity, _buyableMask);
        if (Input.GetKeyDown(_interactKey))
        {
            Debug.Log("aprete e");

            if (colliders.Length > 0)
            {
                print($"Colicioné con {colliders[0].name}");

                if (colliders[0].TryGetComponent(out Interactuable objToBuy))
                {
                    if (BuyEvent(objToBuy.ReturnCost()))
                    {
                        print("Comprando el obj");
                        objToBuy.Buy(this);
                    }
                    else
                    {
                        //Algo en la UI que muestre que no compró
                        Debug.Log("No pudo comprar");
                    }
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_jumpRay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_center.position, _extends);
    }
}
