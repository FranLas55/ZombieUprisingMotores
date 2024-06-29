using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactuable
{
    [SerializeField] string _openName = "onOpen";
    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public override void Buy(Player p)
    {
        print("Se abrió la puerta");
        _animator.SetTrigger(_openName);
    }
}
