using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private SettingsManager settingsManager;

    private Queue<string> spawnEnemies = new Queue<string>();

    private bool timeFrozen = false;

    private bool spawning = false;

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
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
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

    [SerializeField] private float enemySpawnRange;
    [SerializeField] private float enemySpawnX;
    [SerializeField] private float enemySpawnDelay;

    [SerializeField] private GameObject smallEnemy;
    [SerializeField] private GameObject mediumEnemy;
    [SerializeField] private GameObject largeEnemy;

    private Dictionary<string, GameObject> enemyPrefabs;

    public Dictionary<SelectionState, GameObject> Towers
    {
        get
        {
            return towers;
        }
    }

    private int wave = 0;

    private List<Dictionary<string, int>> data;

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

            if (health < 0)
            {
                GameOver();
            }
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
        enemyPrefabs = new Dictionary<string, GameObject>()
        {
            { "small", smallEnemy },
            { "large", largeEnemy },
            { "medium", mediumEnemy }
        };

        towers = new Dictionary<SelectionState, GameObject>()
        {
            { SelectionState.Flamethrower, flamethrower },
            { SelectionState.Cannon, cannon },
            { SelectionState.Mortar, mortar }
        };

        // loads in the values from jsons
        LoadJson();

        // starts the first wave
        Invoke(nameof(LoadWave), firstWaveDelay);
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

        // if nothing is currently spawning
        if (!spawning)
        {
            // and there are enemies to spawn
            if (spawnEnemies.Count > 0)
            {
                // starts spawning them
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        // say that things are spawning
        spawning = true;

        // grabs the next enemy off the queue
        string enemyName = spawnEnemies.Dequeue();

        // spawns the enemy
        GameObject enemy = Instantiate(enemyPrefabs[enemyName], new Vector3 (enemySpawnX, UnityEngine.Random.Range(-enemySpawnRange, enemySpawnRange), -2), Quaternion.identity);

        // sets up the enemy
        enemy.GetComponent<Enemy>().Setup(this);

        // if there are enemies left to spawn
        if (spawnEnemies.Count > 0)
        {
            // spawn it after the delay
            Invoke(nameof(SpawnEnemy), enemySpawnDelay);
        }
        // otherwise
        else
        {
            // say that nothing is spawning anymore
            spawning = false;
        }
    }

    /// <summary>
    /// Loads in the data from the json
    /// </summary>
    private void LoadJson()
    {
        // load the file
        StreamReader stream = new StreamReader(json);

        // read the text
        string text = stream.ReadToEnd();

        // close the stream
        stream.Close();

        // serialize and return the data
        data = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(text);
    }

    private void LoadWave()
    {
        // grabs the waveData from this wave
        Dictionary<string, int> waveData = data[wave];

        // increases the wave
        wave++;

        // gets all the enemies from the wave, and adds them to the queue
        foreach (KeyValuePair<string, int> tuple in waveData)
        {
            // if the first item is saying time till next wave, set it
            if (tuple.Key == "timeDelay")
            {
                Invoke(nameof(LoadWave), tuple.Value);
                continue;
            }

            // adds the number in the tuple copies of the enemy to the spawn queue
            for (int i = 0; i < tuple.Value; i++)
            {
                spawnEnemies.Enqueue(tuple.Key);
            }
        }
    }

    private void GameOver()
    {
        timeFrozen = true;
    }
}
