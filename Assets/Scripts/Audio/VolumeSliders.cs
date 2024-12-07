using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] private bool _enableOnStart = true;
    [Header("UI")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [Header("Values")]
    [Range(0f, 1f)][SerializeField] private float _initMasterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float _initMusicVolume = .2f;
    [Range(0f, 1f)][SerializeField] private float _initSfxVolume = .8f;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        _masterSlider.value = _initMasterVolume;
        _musicSlider.value = _initMusicVolume;
        _sfxSlider.value = _initSfxVolume;

        SetMasterVolume(_initMasterVolume);
        SetMusicVolume(_initMusicVolume);
        SetSFXVolume(_initSfxVolume);

        gameObject.SetActive(_enableOnStart);
    }
    public void SetMasterVolume(float value)
    {
       AudioManager.Instance.SetMasterVolume(value);
    }
    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
