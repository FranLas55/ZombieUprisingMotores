using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer; // Asocia tu Audio Mixer aqu�
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip sliderSound;

    [Header("Audio Mixer Parameters")]
    [SerializeField] private string sfxVolumeParam = "SFXVolume"; // Nombre del par�metro en el Audio Mixer
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string masterVolumeParam = "MasterVolume";

    private Slider attachedSlider;

    private void Start()
    {
        // Intenta obtener el AudioSource si no est� asignado
        if (audioSource == null)
        {
            audioSource = GameObject.Find("Audio Source Botones")?.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource no encontrado. Aseg�rate de que existe en la escena.");
            }
        }

        // Intenta obtener el Slider adjunto (si existe).
        attachedSlider = GetComponent<Slider>();
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

    public void PlaySliderSound()
    {
        if (sliderSound != null && attachedSlider != null)
        {
            // Determina qu� par�metro de volumen se ajusta seg�n el slider asociado.
            string volumeParam = DetermineVolumeParameter();

            if (!string.IsNullOrEmpty(volumeParam))
            {
                float currentVolume;
                if (audioMixer.GetFloat(volumeParam, out currentVolume))
                {
                    // Convierte el valor logar�tmico del mixer a un valor lineal (0 a 1).
                    float linearVolume = Mathf.Pow(10, currentVolume / 20);

                    // Ajusta el volumen del AudioSource.
                    audioSource.volume = linearVolume;
                }
            }

            // Reproduce el sonido del slider.
            audioSource.PlayOneShot(sliderSound);
        }
    }

    private string DetermineVolumeParameter()
    {
        // Dependiendo del slider, retorna el par�metro correspondiente.
        if (attachedSlider.name.Contains("SFX", System.StringComparison.OrdinalIgnoreCase))
        {
            return sfxVolumeParam;
        }
        else if (attachedSlider.name.Contains("Music", System.StringComparison.OrdinalIgnoreCase))
        {
            return musicVolumeParam;
        }
        else if (attachedSlider.name.Contains("Master", System.StringComparison.OrdinalIgnoreCase))
        {
            return masterVolumeParam;
        }
        else
        {
            Debug.LogWarning("Slider no est� asociado a un par�metro conocido.");
            return null;
        }
    }
}
