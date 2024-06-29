using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _points;
    private int _enemyCount;
    [SerializeField] private Zombie[] _zombiePrefabs;
    [SerializeField] private Player _player;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _bulletsText;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _lifeBar;

    public delegate void VoidDelegate();

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
        if (Input.GetKeyDown(KeyCode.V)) AddPoints(300);
    }

    private void SpawnEnemys()
    {
        //SISTEMA DE RONDAS O CON ALGUN BOTON QUE APAREZCAN ALGUNOS ZOMBIES
        //asignarle por a un evento (Cuando muere) de Zombie el metodo AddPoints
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
        print("El player murió. Fin del juego");
    }
}
