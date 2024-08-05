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
    [Header("Spawner")]
    [SerializeField] private Zombie[] _zombiePrefabs;
    [SerializeField] private Player _player;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _roundDelay;
    [SerializeField] private bool _canStartSpawning;
    [SerializeField, Tooltip("Number of zombies to be spawned")] private int _zombiesToSpawn;
    [SerializeField] private PlayerCamera _playerCamera;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _bulletsText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _lifeBar;
    public Canvas gameOverCanvas;
    public Canvas winCanvas;

    [Header("Cheats")]
    [SerializeField, Tooltip("Enable/Disable rounds")] private KeyCode _roundKey = KeyCode.L;
    [SerializeField, Tooltip("Start the round")] private KeyCode _spawnKey = KeyCode.O;
    [SerializeField, Tooltip("Gives you a specified amount of points")] private KeyCode _pointsKey = KeyCode.V;
    [SerializeField, Tooltip("Amount of points")] private int _cheatPoints;

    [Header("Buyable")] 
    [SerializeField] private string[] _buyKeys = new string[7];
    [SerializeField] private int[] _buyValues = new int[7];
    
    // 3 door + 3 weapon stand + 1 health kit = 7


    Dictionary<string, int> _buyDictionary = new();
    private bool _startRoundsAtStart;

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
        _player.GameOverEvent += GameOver;
        AddPoints(0);

        for (int i = 0; i < _buyKeys.Length; i++)
        {
            _buyDictionary.Add(_buyKeys[i], _buyValues[i]);
            
            //print($"key = {_buyKeys[i]} value = {buyDictionary[_buyKeys[i]]}");
        }

        if (_canStartSpawning)
        {
            _startRoundsAtStart = true;
        }

        gameOverCanvas.enabled = false;
        winCanvas.enabled = false;
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
        if (Input.GetKeyDown(_spawnKey) || _startRoundsAtStart)
        {
            //Spawnear zombiesToSpawn una sola vez
            _startRoundsAtStart = false;
            StartCoroutine(SpawnCor(false));
        }
    }

    List<Zombie> _zombiesInScene = new List<Zombie>();

    private bool _isSpawning;
    IEnumerator SpawnCor(bool hasDelay)
    {
        if (hasDelay) yield return new WaitForSeconds(_roundDelay);

        print("Comenz� la ronda");
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

        _points = 0;
        AddPoints(0);
        
        _player.enabled = false;

        _playerCamera.enabled = false;


        if (_player.HasDied)
        {
            if (gameOverCanvas != null)
            {
                gameOverCanvas.enabled = true;
            }
        }
        else
        {
            if (winCanvas != null)
            {
                winCanvas.enabled = true;
            }
        }

       

       /* Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;*/

        _startRoundsAtStart = true;
        this.enabled = false;

        print("El player murio. Fin del juego");
    }
    public void Win()
    {
        StopAllCoroutines();

        foreach (var item in _zombiesInScene)
        {
            Destroy(item.gameObject);
        }

        _points = 0;
        AddPoints(0);

        _player.enabled = false;

        _playerCamera.enabled = false;


        if (winCanvas != null)
        {
            winCanvas.enabled = true;
        }

        /* Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;*/

        _startRoundsAtStart = true;
        this.enabled = false;

        print("El player gano. Fin del juego");
    }


    /// <summary>
    /// Asks for the value of a key on the buyDictionary
    /// </summary>
    /// <param name="key">key name on the dictionary</param>
    /// <returns>if the key exists returns its value. If not, returns the max value of int in order to not buy the item</returns>
    public int GetKeyValue(string key)
    {
        if (_buyDictionary.ContainsKey(key))
        {
            return _buyDictionary[key];
        }
        
        
        return int.MaxValue;
    }
}
