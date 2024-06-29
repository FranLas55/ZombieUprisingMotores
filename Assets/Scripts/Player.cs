using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

//Francisco Lastra

[RequireComponent(typeof(Rigidbody))]
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

    [Header("JumpRay")]
    [SerializeField] private float _jumpRayRange = .5f;
    [SerializeField] private LayerMask _jumpMask;

    private Vector3 _dir = new(), _transformOffset = new();

    private Rigidbody _rb;

    private Movement _movement;

    private Ray _jumpRay;

    protected override void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = new Movement(_rb, _speed);
        base.Start();
    }

    private void Update()
    {
        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(_runKey)) _movement.ChangeSpeed(_runSpeed);
        else _movement.RestartSpeed();

        if (IsGrounded() && Input.GetKeyDown(_jumpKey)) Jump();
    }

    private void FixedUpdate()
    {
        _movement.Move(_dir);
    }

    protected override void OnDeath()
    {
        
    }

    public void ChangeWeapon(WeaponEnum newWeapon)
    {
        print($"Cambió de arma, la nueva arma es {newWeapon}");
        _weapon = newWeapon;

        foreach(var weapon in _myWeapons)
        {
            //Fijarse si el enum del arma es igual a _weapon y si es el mismo prenderla, si no apagarla
        }
    }


    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3 (transform.position.x,
                                        transform.position.y + _jumpRayRange / 4,
                                        transform.position.z);

        _jumpRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_jumpRay, _jumpRayRange, _jumpMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_jumpRay);
    }
}
