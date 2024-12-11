using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnPassage : MonoBehaviour
{
    [Header("Escena")]
    [Tooltip("Nombre de la escena a cargar.")]
    [SerializeField] private string _sceneToLoad;

    private bool _sceneLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_sceneLoaded && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            LoadSceneAdditively();
        }
    }

    private void LoadSceneAdditively()
    {
        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
            _sceneLoaded = true;
            Debug.Log($"Escena cargada correctamente");
        }
        else
        {
            Debug.LogWarning("No hay escena");
        }
    }
}

