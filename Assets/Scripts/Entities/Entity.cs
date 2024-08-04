using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//Francisco Lastra

[RequireComponent(typeof(Rigidbody))]
public abstract class Entity : MonoBehaviour, IDamageable
{
    [Header("Values")]
    [SerializeField] protected int _hp;
    [SerializeField] protected float _speed;
    
    [SerializeField] private float _wallRayRange = .6f;
    [SerializeField] private LayerMask _wallMask;

    private Ray _wallRay;
    protected int _actualHp;
    protected Rigidbody _rb;

    protected Movement _movement;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _actualHp = _hp;
        _movement = new Movement(_rb, _speed);
    }

    public virtual void Heal(int hp)
    {
        _actualHp += hp;

        if(_actualHp > _hp)
        {
            _actualHp = _hp;
        }

        print($"{gameObject.name} se curo {hp}. Vida actual: {_actualHp}");
    }

    public virtual void TakeDamage(int dmg)
    {
        _actualHp -= dmg;

        if (_actualHp <= 0)
        {
            //se muere
            OnDeath();
        }
    }
    /*protected virtual void Update()
    {

    }*/

    public abstract void OnDeath();
    
    protected bool IsBlocked(Vector3 dir)
    {
        _wallRay = new Ray(transform.position, dir.normalized);

        return Physics.Raycast(_wallRay, _wallRayRange, _wallMask);
    }
}
