using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private SettingsManager settingsManager;

    private Queue<Enemy> spawnEnemies = new Queue<Enemy>();

    private bool timeFrozen = false;

    /// <summary>
    /// Freezes / Unfreezes time
    /// </summary>
    public bool TimeFrozen
    {
        get
        {
            return timeFrozen;
        }

        set
        {
            // freezes or unfreezes time
            if (value == false)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

            // changes the value
            timeFrozen = value;
        }
    }

    [SerializeField] private List<GameObject> enemyPrefab;
    [SerializeField] private string json;

    [SerializeField] private float firstWaveDelay;

    [SerializeField] internal UIManager uIManager;

    [SerializeField] private GameObject flamethrower;
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject mortar;
    [SerializeField] private Dictionary<SelectionState, GameObject> towers;

    public Dictionary<SelectionState, GameObject> Towers
    {
        get
        {
            return towers;
        }
    }

    private int wave = 0;

    private List<Dictionary<string, string>> data;

    [SerializeField] private int money = 0;

    public int Money
    {
        get 
        { 
            return money; 
        }
        set 
        { 
            money = value;
            uIManager.UpdateMoneyText(money);
        }
    }

    [SerializeField] private float health;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            uIManager.UpdateHealthText(health);
        }
    }

    /// <summary>
    /// The list of all enemies in the game at that moment
    /// </summary>
    public List<Enemy> Enemies
    {
        get
        {
            return enemies;
        }
    }

    private void Start()
    {
        towers = new Dictionary<SelectionState, GameObject>()
        {
            { SelectionState.Flamethrower, flamethrower },
            { SelectionState.Cannon, cannon },
            { SelectionState.Mortar, mortar }
        };

        // loads in the values from jsons
        //LoadJson();

        // starts the first wave
        //Invoke(nameof(LoadWave), firstWaveDelay);
    }

    private void Update()
    {
        // if the player presses the escape key, the menu is activated
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsManager.gameObject.active)
            {
                settingsManager.Resume();
            }
            else
            {
                settingsManager.gameObject.SetActive(true);
                settingsManager.Activate();
            }
        }
    }

    /// <summary>
    /// Loads in the data from the json
    /// </summary>
    private void LoadJson()
    {
        data = json.LoadJsonData();
    }

    private void LoadWave()
    {
        // grabs the waveData from this wave
        Dictionary<string, string> waveData = data[wave];

        // increases the wave
        wave++;
    }
}
