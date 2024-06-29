using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : Interactuable
{
    [SerializeField] int _hpToHeal;

    public override void Buy(Player p) //HealPlayer
    {
        p.Heal(_hpToHeal);
    }
}
