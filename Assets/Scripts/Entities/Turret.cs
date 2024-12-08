using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _shootPoint;
    
    [Header("Stats")]
    [SerializeField] private int _maxHP;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _detectionRadius;
    
    [Header("Masks")]
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;

    [Header("Bullet")]
    [SerializeField] private int _damage;
    [SerializeField] private ProyectileBullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed;

    [Header("Particles")] 
    [SerializeField] private ParticleSystem _shotParticles;
    [SerializeField] private ParticleSystem _damageParticles;
    [SerializeField] private ParticleSystem _explosionPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioClip _damageClip;

    private ParticleAudio _shotAudio;
    private ParticleAudio _damageAudio;
    
    private int _currentHp;
    private float _curretTime;
    private float TimeBetweenBullet => 1/ _fireRate;
    private Transform _target;
    private void Start()
    {
        _currentHp = _maxHP;

        _curretTime = TimeBetweenBullet;
        _shotAudio = _shotParticles.GetComponent<ParticleAudio>();
        _damageAudio = _damageParticles.GetComponent<ParticleAudio>();
    }

    private void Update()
    {
        if (!_target) return;

        if (_curretTime <= 0)
        {
            Shoot();
            _curretTime = TimeBetweenBullet;
        }
        else
        {
            _curretTime -= Time.deltaTime;
        }

        var dir = _target.position - transform.position;
        dir.y = 0;

        transform.forward = dir;
    }

    private void FixedUpdate()
    {
        var possibleTargets = Physics.OverlapSphere(transform.position, _detectionRadius, _targetMask);

        if (possibleTargets.Length <= 0)
        {
            _target = null;
            return;
        }

        var dir = possibleTargets[0].transform.position - transform.position;

        _target = !Physics.Raycast(transform.position, dir.normalized, dir.magnitude, _obstacleMask) 
            ? possibleTargets[0].transform : null;
    }

    public void Shoot()
    {
        if (!_target) return;
        _shotParticles.Play();
        _shotAudio.PlayClip(_shotClip);
        var bullet = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);

        bullet.InitializeBullet(_damage, 5f, _bulletSpeed);
        bullet.transform.forward = _target.position - _shootPoint.position;
    }

    public void TakeDamage(int dmg)
    {
        _currentHp -= dmg;
        _damageParticles.Play();
        _damageAudio.PlayClip(_damageClip);

        if (_currentHp <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        var exp = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        exp.transform.localScale = Vector3.one * 3;
        exp.Play();
        exp.GetComponent<ParticleAudio>().PlayClip();
        Destroy(exp, 3f);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
