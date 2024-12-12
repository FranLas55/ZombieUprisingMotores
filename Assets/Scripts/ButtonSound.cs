using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    void Start()
    {
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void PlayHoverSound()
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
