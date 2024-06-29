using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float scale;
    [SerializeField] LayerMask _bulletMask;
    private int _damage = 0;

    private Rigidbody _rb;

    private float _lifeTime;
    private float _speed;

    private Movement _movement;
    private Collider _target;

    //private Entity _ignoreType;

    //SetDamage y SetDir en uno solo
    public void InitializeBullet(int damage, float lifeTime, float speed/*, Entity shooter*/)
    {
        _damage = damage;
        _lifeTime = lifeTime;
        _speed = speed;
        //_ignoreType = shooter;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        Destroy(gameObject, _lifeTime);

        _movement = new Movement(_rb, _speed);
    }

    private void FixedUpdate()
    {
        _movement.Move(transform.forward);
        //Que le haga daño a IDamageable MENOS al tipo que la disparó, si un zombie dispara, no hace daño a zombie

        _target = HitObject();

        if (_target != null)
        {
            //hace daño a IDamageable
            Destroy(gameObject);
        }
    }

    private Collider HitObject()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, scale, _bulletMask);
        
        if (objects.Length <= 0)
        {
            return null;
        }
        else
        {
            return objects[0];
        }
    }
}

