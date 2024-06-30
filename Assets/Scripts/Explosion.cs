using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float maxScale = 5f;
    public float growthRate = 1f;
    public int damage = 10;
    public float duration = 3f;

    private float currentTime = 0f;

    private void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float scale = Mathf.Lerp(0, maxScale, currentTime / duration);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity player = other.GetComponent<Entity>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public float GetRadius()
    {
        return maxScale;
    }
}
