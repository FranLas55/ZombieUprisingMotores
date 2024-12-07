using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [Header("Audio")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    public void SetMasterVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        _mixer.SetFloat("MasterVolume", volume);
    }
    public void SetSFXVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        _mixer.SetFloat("SFXVolume", volume);
    }
    public void SetMusicVolume(float value)
    {
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        _mixer.SetFloat("MusicVolume", volume);
    }
    public void PlayClip(AudioClip clip)
    {
        if (_source.clip == clip) return;
        if(_source.isPlaying) _source.Stop();
        _source.clip = clip;   
        _source.Play();
    }


}
