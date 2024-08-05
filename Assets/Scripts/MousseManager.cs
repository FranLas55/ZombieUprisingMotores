using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousseManager : MonoBehaviour
{
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Canvas _winCanvas;
   

    private void Update()
    {
        if (_gameManager.gameOverCanvas.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (_gameManager.winCanvas.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }




    }
}
