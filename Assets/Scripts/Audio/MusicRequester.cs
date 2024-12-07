using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
           AudioManager.Instance.PlayClip(_clip);
    }
}
