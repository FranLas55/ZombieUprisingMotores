using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public class HealthKit : Interactuable
{
    [SerializeField] int _hpToHeal;
    [SerializeField] private AudioClip _healClip;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public override void Buy(Player p) //HealPlayer
    {
        _source.PlayOneShot(_healClip);
        p.Heal(_hpToHeal);
    }
}
