using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Canvas[] _mainMenuScreens;

    private Canvas _actualScreen;

    private void Start()
    {
        Cursor.visible = true;

        if (_mainMenuScreens.Length > 0)
        {
            for (int i = 0; i < _mainMenuScreens.Length; i++)
            {
                if (i > 0)
                {
                    _mainMenuScreens[i].enabled = false;
                }
                else
                {
                    _actualScreen = _mainMenuScreens[i];
                }
            }
        }
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeCanvas(int index)
    {
        if (_mainMenuScreens.Length > 0)
        {
            _actualScreen.enabled = false;
            _actualScreen = _mainMenuScreens[index];
            _actualScreen.enabled = true;
        }
    }

    public void CloseUP()
    {
        Application.Quit();
    }
}
