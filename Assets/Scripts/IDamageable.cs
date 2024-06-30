using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carlos Coronel

public interface IDamageable
{
    public void TakeDamage(int dmg);

    public void OnDeath();
}
