using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Canvas[] _mainMenuScreens;
    [SerializeField] private GameObject loadingScreen;

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

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
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

    public void ChangeSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
