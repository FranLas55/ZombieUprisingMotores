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
    [SerializeField] private LayerMask _dmgMask;

    private float currentTime = 0f;

    private float currentScale;

    private List<IDamageable> _damageables = new();

    private void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            currentScale = Mathf.Lerp(0, maxScale, currentTime / duration);
        }
        
        if(currentScale >= maxScale)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        var explotable = Physics.OverlapSphere(transform.position, currentScale, _dmgMask);

        if (explotable.Length <= 0) return;

        foreach (var entity in explotable)
        {
            if (entity.TryGetComponent(out IDamageable dmg) && !_damageables.Contains(dmg))
            {
                dmg.TakeDamage(damage);
                _damageables.Add(dmg);
            }
        }
    }

    public void SetRadius(float radius)
    {
        maxScale = radius;
    }
}
