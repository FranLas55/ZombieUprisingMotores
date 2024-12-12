using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        _source?.PlayOneShot(clip);
    }
    
    public void PlayClip()
    {
        _source?.PlayOneShot(_clip);
    }
}
