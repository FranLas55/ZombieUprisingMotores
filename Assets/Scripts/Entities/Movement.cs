using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public class Movement
{
    Rigidbody _rb;
    float _speed;

    float _ogSpeed;
    private bool _canMove;

    public Movement(Rigidbody rb, float speed)
    {
        _rb = rb;
        _speed = speed;
        _ogSpeed = speed;
        _canMove = true;
    }

    public void Move(Vector3 dir)
    {
        if (!_canMove) return;
        if (dir.magnitude <= .5f) return;
        _rb.MovePosition(_rb.transform.position + (dir.normalized * Time.deltaTime * _speed));
    }

    public void ChangeSpeed(float speed)
    {
        _speed = speed;
    }

    public void RestartSpeed()
    {
        _speed = _ogSpeed;
    }

    public void Stop()
    {
        _canMove = false;
    }
}
