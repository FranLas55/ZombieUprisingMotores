using UnityEngine;

public class FootController : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip[] _footstep;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        _source.PlayOneShot(_footstep[Random.Range(0, _footstep.Length)]);
    }
}
