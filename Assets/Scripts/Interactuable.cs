using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public abstract class Interactuable : MonoBehaviour 
{
    [SerializeField] int _cost = 10;

    public abstract void Buy(Player player);

    public int ReturnCost()
    {
        return _cost;
    }
}
