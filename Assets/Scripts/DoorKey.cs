using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Francisco Lastra

public class DoorKey : MonoBehaviour
{
    private Door _door;
    private Animator _animator;

    private bool coso;

    private void Start()
    {
        _door = GetComponent<Door>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_door.hasBeenBought && !coso)
        {
            _animator.SetTrigger("GetDown");
            LoadSceneAdditively();
            coso = true;
        }
    }
    
    [Header("Escena")]
    [Tooltip("Nombre de la escena a cargar.")]
    [SerializeField] private string _sceneToLoad;

    private void LoadSceneAdditively()
    {
        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
            Debug.Log($"Escena cargada correctamente");
        }
        else
        {
            Debug.LogWarning("No hay escena");
        }
    }
}
