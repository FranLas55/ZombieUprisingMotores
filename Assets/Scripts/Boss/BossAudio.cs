using UnityEngine;

public class BossAudio : MonoBehaviour
{
    [Header("Audio Source")] 
    [SerializeField] private AudioSource _footSource;
    [SerializeField] private AudioSource _mouthSource;

    [Header("SFX")] 
    [SerializeField] private AudioClip[] _footStep; //1, 2
    [SerializeField] private AudioClip _punch;//6
    [SerializeField] private AudioClip _grab;//7
    [SerializeField] private AudioClip _throw;//5
    [SerializeField] private AudioClip _groundPunch;//3
    [SerializeField] private AudioClip _groundGrowl;//5
    [SerializeField] private AudioClip _changePhase;//4
    [SerializeField] private AudioClip _death;//8

    public void PlayRandomStep()
    {
        _footSource.PlayOneShot(_footStep[Random.Range(0, _footStep.Length)]);
    }

    public void PunchSfx()
    {
        _mouthSource.PlayOneShot(_punch);
    }

    public void GrabSfx()
    {
        _mouthSource.PlayOneShot(_grab);
    }

    public void ThrowSfx()
    {
        _mouthSource.PlayOneShot(_throw);
    }

    public void ChangePhase()
    {
        _mouthSource.PlayOneShot(_changePhase);
        
    }

    public void GroundPunchSfx()
    {
        _footSource.PlayOneShot(_groundPunch);
    }

    public void GroundGrowlSfx()
    {
        _mouthSource.PlayOneShot(_groundGrowl);
    }

    public void DeathSfx()
    {
        _mouthSource.PlayOneShot(_death);
    }
}