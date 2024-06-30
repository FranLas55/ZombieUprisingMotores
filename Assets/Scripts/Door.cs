using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public class Door : Interactuable
{
    [SerializeField] string _openName = "onOpen";
    [SerializeField] string _resetName = "onRestart";
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Player.Instance.PlayerDead += Restart;
    }


    public override void Buy(Player p)
    {
        print("Se abrió la puerta");
        _animator.SetTrigger(_openName);
    }


    private void Restart()
    {
        _animator.SetTrigger(_resetName);
    }
}
