using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Tobias

public class GameManager : MonoBehaviour
{
    private int _points;
    private int _enemyCount;
    [SerializeField] private Zombie[] _zombiePrefabs;
    [SerializeField] private Player _player;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _roundDelay;
    [SerializeField] private bool _canStartSpawning;
    [SerializeField, Tooltip("Number of zombies to be spawned")] private int _zombiesToSpawn;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _bulletsText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _lifeBar;

    [Header("Cheats")]
    [SerializeField, Tooltip("Enable/Disable rounds")] private KeyCode _roundKey = KeyCode.L;
    [SerializeField, Tooltip("Start the round")] private KeyCode _spawnKey = KeyCode.O;
    [SerializeField, Tooltip("Gives you a specified amount of points")] private KeyCode _pointsKey = KeyCode.V;
    [SerializeField, Tooltip("Amount of points")] private int _cheatPoints;

    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        _player = Player.Instance;

        _player.BuyEvent += Buy;
        _player.PlayerDead += GameOver;
        AddPoints(0);
    }

    public void AddPoints(int points)
    {
        _points += points;
        //Actualizar UI
        _pointsText.text = $"Puntos: {_points}";
    }

    private void Update()
    {
        //prueba
        if (Input.GetKeyDown(_pointsKey)) AddPoints(_cheatPoints);

        if (Input.GetKeyDown(_roundKey)) _canStartSpawning = !_canStartSpawning;

        SpawnEnemys();
    }

    private void SpawnEnemys()
    {
        //SISTEMA DE RONDAS O CON ALGUN BOTON QUE APAREZCAN ALGUNOS ZOMBIES
        //asignarle por a un evento (Cuando muere) de Zombie el metodo AddPoints
        if (Input.GetKeyDown(_spawnKey))
        {
            //Spawnear zombiesToSpawn una sola vez
            StartCoroutine(SpawnCor(false));
        }
    }

    List<Zombie> _zombiesInScene = new List<Zombie>();

    private bool _isSpawning;
    IEnumerator SpawnCor(bool hasDelay)
    {
        if (hasDelay) yield return new WaitForSeconds(_roundDelay);

        print("Comenzó la ronda");
        int z = 0;
        _isSpawning = true;

        while (z < _zombiesToSpawn)
        {
            Zombie newZombie = Instantiate(_zombiePrefabs[Random.Range(0, _zombiePrefabs.Length)],
                SelectPoint().position, Quaternion.identity);
            newZombie.InitializeZombie(_player.transform);

            _zombiesInScene.Add(newZombie);
            print("zombies en escena = " + _zombiesInScene.Count);

            z++;
            if (z < _zombiesToSpawn) yield return new WaitForSeconds(_spawnDelay);
            else yield return null;
        }
        _isSpawning = false;
    }

    private Transform SelectPoint()
    {
        Transform point;

        do
        {
            point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        } while (point.gameObject.activeSelf == false);

        return point;
    }

    public void RemoveZombie(Zombie z)
    {
        if (_zombiesInScene.Contains(z))
        {
            print("Elimino zombie");
            _zombiesInScene.Remove(z);

            if (_isSpawning == true) return;
            if (_zombiesInScene.Count <= 0 && _canStartSpawning == true)
            {
                print("Reiniciando");
                StopAllCoroutines();
                print("Empezando(?");
                StartCoroutine(SpawnCor(true));
            }
        }
    }

    public void UpdateLifeBar(float value)
    {
        value = Mathf.Clamp01(value);

        _lifeBar.fillAmount = value;
    }

    public void UpdateBullets(int actualBullets, int maxBullets, int actualAmmo, bool isInfinite)
    {
        _bulletsText.text = $"{actualBullets} / {maxBullets}";
        if (isInfinite)
        {
            _ammoText.text = $"";
        }
        else
        {
            _ammoText.text = $"{actualAmmo}";
        }
    }

    private bool Buy(int pointsNeed)
    {
        bool canBuy;

        if (pointsNeed <= _points)
        {
            _points -= pointsNeed;
            //actualizar UI
            _pointsText.text = $"Puntos: {_points}";
            canBuy = true;
        }
        else canBuy = false;

        return canBuy;
    }

    private void GameOver()
    {
        StopAllCoroutines();

        foreach (var item in _zombiesInScene)
        {
            Destroy(item.gameObject);
        }

        print("El player murió. Fin del juego");
    }
}
