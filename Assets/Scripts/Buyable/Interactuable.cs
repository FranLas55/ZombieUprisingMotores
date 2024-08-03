using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public abstract class Interactuable : MonoBehaviour 
{
    [SerializeField] string _key = "";

    public abstract void Buy(Player player);

    public string ReturnKey()
    {
        return _key;
    }
}
