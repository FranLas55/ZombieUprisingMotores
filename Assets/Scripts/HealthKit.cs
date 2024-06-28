using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : Interactuable
{
    [SerializeField] int _hpToHeal;

    public override void Buy(Player p) //HealPlayer
    {
        //preguntarle al GM si el jugador tiene los puntos suficientes para comprarla
            p.Heal(_hpToHeal);
            //Restar puntos
    }
}
