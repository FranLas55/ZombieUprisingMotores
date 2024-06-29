using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
        private int _damage = 0;

        private Rigidbody _rb;

        private float _lifeTime;

        //SetDamage y SetDir en uno solo
        public void InitializeBullet(int damage, float lifeTime)
        {
            _damage = damage;
            _lifeTime = lifeTime;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }


        public void Shot(Vector3 dir)
        {
            //Destroy(gameObject, _lifeTime);
            transform.SetParent(null);
            _rb.constraints = RigidbodyConstraints.None;

            _rb.velocity = dir;
        }


        /*private void OnCollisionEnter(Collision collision)
        {
            //hacer daño al enemigo
            if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
            {
                player.TakeDamage(_damage);
                print($"Le pegue al player");
            }

            Destroy(gameObject);
        }*/
    }

