using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Francisco Lastra

public class Door : Interactuable
{
    [SerializeField] string _openName = "onOpen";
    [SerializeField] string _resetName = "onRestart";
    [SerializeField] Animator _animator;
    [SerializeField] private AudioSource _source;

    public bool hasBeenBought { get; private set; }

    private void Start()
    {
        hasBeenBought = false;
        if(_animator == null) _animator = GetComponent<Animator>();
        Player.Instance.GameOverEvent += Restart;
    }


    public override void Buy(Player p)
    {
        print("Se abrió la puerta");
        _animator.SetTrigger(_openName);
        _source.Play();
        hasBeenBought = true;
    }


    private void Restart()
    {
        _animator.SetTrigger(_resetName);
        hasBeenBought = false;
    }
}
