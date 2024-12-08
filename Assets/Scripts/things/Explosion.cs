using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carlos Coronel

public class Explosion : MonoBehaviour
{
    private float maxScale = 5f;

    [Header("Values")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float duration = 1f;

    private float currentTime = 0f;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponentInChildren<Collider>();
    }

    private void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float scale = Mathf.Lerp(0, maxScale, currentTime / duration);
            _collider.transform.localScale = new Vector3(scale, scale, scale);
        }
        
        if(_collider.transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable entity))
        {
            entity.TakeDamage(damage);
        }
    }

    public void SetRadius(float radius)
    {
        maxScale = radius;
    }
}
