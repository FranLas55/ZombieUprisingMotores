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
}
