using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{
    [SerializeField] private Canvas _canvasToActive;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerCamera _camera;
    [SerializeField] private GameManager _gameManager;
    
   
    private void Start()
    {
        if (_canvasToActive != null)
        {
            _canvasToActive.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.GetComponent<Player>())
       {
            _canvasToActive.enabled = false;
            _player.enabled = false;
            _camera.enabled = false;
            _gameManager.enabled = false;
            ActiveCanvas();
        }
    }

    private void ActiveCanvas()
    {
        if ( _canvasToActive != null )
        {
            _canvasToActive.enabled = true;
        }
    }
}
