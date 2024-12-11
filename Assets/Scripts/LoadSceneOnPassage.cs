using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnPassage : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the scene to load additively.")]
    [SerializeField] private string sceneToLoad;

    private bool sceneLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!sceneLoaded && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadSceneAdditively();
        }
    }

    private void LoadSceneAdditively()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            sceneLoaded = true;
            Debug.Log($"Scene '{sceneToLoad}' loaded additively.");
        }
        else
        {
            Debug.LogWarning("No scene name specified to load.");
        }
    }
}

