using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

//Francisco Lastra

public abstract class Entity : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int _hp;
    [SerializeField] protected float _speed;

    protected int _actualHp;

    protected virtual void Start()
    {
        _actualHp = _hp;
    }

    public void Heal(int hp)
    {
        _actualHp += hp;
        print($"{gameObject.name} se curo {hp}. Vida actual: {_actualHp}");
    }

    public void TakeDamage(int dmg)
    {
        _actualHp -= dmg;

        if(_actualHp <= 0)
        {
            //se muere
            OnDeath();
        }
    }

    protected abstract void OnDeath();
}
