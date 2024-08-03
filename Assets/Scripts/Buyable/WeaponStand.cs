using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

//Francisco Lastra

public class WeaponStand : Interactuable
{
    [Tooltip("If it has more than one weapon is because it is a chest")]
    [SerializeField] WeaponEnum[] _weapons;
    [Tooltip("If is chest when the player buys it give him a random weapon")]
    [SerializeField] bool _isChest;

    public override void Buy(Player p)
    {
        if (_isChest)
        {
            var weapon = _weapons[Random.Range(0, _weapons.Length)];
            p.ChangeWeapon(weapon);
        }
        else
        {
            p.ChangeWeapon(_weapons[0]);
        }
    }
}
