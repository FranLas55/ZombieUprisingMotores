using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public class Movement
{
    Rigidbody _rb;
    float _speed;

    float _ogSpeed;

    public Movement(Rigidbody rb, float speed)
    {
        _rb = rb;
        _speed = speed;
        _ogSpeed = speed;
    }

    public void Move(Vector3 dir)
    {
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
}
