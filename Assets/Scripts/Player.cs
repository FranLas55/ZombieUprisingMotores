using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity
{
    [Header("Values")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private int _shield;
    [SerializeField] private float _jumpForce;
    [SerializeField] private WeaponEnum _weapon = WeaponEnum.Gun;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

    private Vector3 _dir = new();

    private Movement _movement;

    protected override void Start()
    {
        _movement = new Movement(GetComponent<Rigidbody>(), _speed);
        base.Start();
    }

    private void Update()
    {
        _dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(_runKey)) _movement.ChangeSpeed(_runSpeed);
        else _movement.RestartSpeed();
    }

    private void FixedUpdate()
    {
        _movement.Move(_dir);
    }

    protected override void OnDeath()
    {
        
    }
}
