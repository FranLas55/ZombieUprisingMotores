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
        //preguntarle al GM si el jugador tiene los puntos suficientes para comprarla
            print("Se abrió la puerta");
            _animator.SetTrigger(_openName);
            //RestarPuntos
    }
}
