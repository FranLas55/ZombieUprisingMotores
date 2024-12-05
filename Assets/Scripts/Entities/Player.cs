using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

//Francisco Lastra

[System.Serializable]
public struct PlayerInputs
{
    public KeyCode interactKey;
    public KeyCode runKey;
    public KeyCode jumpKey;
    public KeyCode rechargeKey;
}


public class Player : Entity
{
    [Header("Values")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private WeaponEnum _weapon = WeaponEnum.Gun;
    [SerializeField] private Weapon[] _myWeapons;

    [Header("Inputs")]
    [SerializeField]private PlayerInputs _inputs;


    [Header("InteractBox")]
    [SerializeField] private Vector3 _extends;
    [SerializeField] private Transform _center;
    [SerializeField] private LayerMask _buyableMask;

    [Header("Rays")]
    [SerializeField] private float _jumpRayRange = .5f;
    [SerializeField] private LayerMask _jumpMask;
    
    
    private Vector3 _dir = new(), _transformOffset = new();

    private Ray _jumpRay;

    private Weapon _actualWeapon;

    public delegate void VoidDelegate();

    public event VoidDelegate GameOverEvent;

    public delegate bool Buy(int i);
    public Buy BuyEvent;
    
    public bool HasDied { get; private set; }


    #region Singleton
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        ChangeWeapon(WeaponEnum.Gun);
        GameOverEvent += ResetPlayer; 
    }

    private void ResetPlayer()
    {
        //SceneManager.LoadScene(0);
        Heal(_hp);
        HasDied = false;
        ChangeWeapon(WeaponEnum.Gun);
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        _dir = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(_inputs.runKey)) _movement.ChangeSpeed(_runSpeed);
        else _movement.RestartSpeed();

        if (IsGrounded() && Input.GetKeyDown(_inputs.jumpKey)) Jump();

        if (Input.GetMouseButtonDown(0))
        {
            _actualWeapon.Shoot();
        }

        if (Input.GetKeyDown(_inputs.rechargeKey)) _actualWeapon.Recharge();

        BuyObj();

        //pruebas
        if (Input.GetKeyDown(KeyCode.M)) ChangeWeapon(WeaponEnum.Famas);
        if (Input.GetKeyDown(KeyCode.N)) ChangeWeapon(WeaponEnum.ShotGun);
        if (Input.GetKeyDown(KeyCode.G)) ChangeWeapon(WeaponEnum.Gun);
        if (Input.GetKeyDown(KeyCode.C)) TakeDamage(1);

    }

    public void Win()
    {
        GameOverEvent();
    }

    private void FixedUpdate()
    {
        if (IsBlocked(_dir)) _dir = Vector3.zero;
        _movement.Move(_dir);
    }

    public override void OnDeath()
    {
        //Puede pasar algo como no
        HasDied = true;
        GameOverEvent();
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
        if (Input.GetKeyDown(_inputs.interactKey))
        {
            Debug.Log("aprete e");

            if (colliders.Length > 0)
            {
                print($"Colicioné con {colliders[0].name}");

                if (colliders[0].TryGetComponent(out Interactuable objToBuy))
                {
                    if (BuyEvent(GameManager.Instance.GetKeyValue(objToBuy.ReturnKey())))
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
