using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    private Door _door;
    private Renderer _renderer;
    private Collider _collider;

    private void Start()
    {
        _door = GetComponent<Door>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        _renderer.enabled = !_door.hasBeenBought;
        _collider.enabled = !_door.hasBeenBought;
    }
}
