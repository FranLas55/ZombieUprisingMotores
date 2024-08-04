using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerCamera _playerCamara;

    public void RestartGame()
    {
        if (_gameManager != null)
        {
            _gameManager.enabled = true;
            _player.enabled = true;
            _playerCamara.enabled = true;


            if (_gameManager.gameOverCanvas != null)
            {
                _gameManager.gameOverCanvas.enabled = false;
            }
        }


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
