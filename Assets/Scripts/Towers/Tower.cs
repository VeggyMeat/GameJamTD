using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private string towerName;

    /// <summary>
    /// The name of the tower
    /// </summary>
    public string TowerName
    {
        get
        {
            return towerName;
        }
    }

    private TowerSlot towerSlot;

    protected GameManager gameManager;

    protected float minRange;
    protected float maxRange;

    protected float attackDelay;

    [SerializeField] protected float scanDelay;

    protected DateTime lastAttackTime;

    [SerializeField] protected string projectileJson;

    protected List<Dictionary<string, string>> projectileData;

    [SerializeField] protected string json;

    protected List<Dictionary<string, string>> data;

    [SerializeField] protected GameObject projectilePrefab;

    protected int level = 0;

    /// <summary>
    /// The level of the tower (from 0 to 2)
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
    }

    protected bool jsonsLoaded;

    /// <summary>
    /// Returns the number of seconds since the last attack
    /// </summary>
    protected float TimeSinceLastAttack
    {
        get
        {
            return DateTime.Now.Subtract(lastAttackTime).Seconds;
        }
    }

    /// <summary>
    /// Returns whether or not the tower is able to attack from being off cooldown
    /// </summary>
    public bool CanAttack
    {
        get
        {
            return TimeSinceLastAttack >= attackDelay;
        }
    }

    /// <summary>
    /// Sets up the tower
    /// </summary>
    /// <param name="towerSlot">The towerSlot in which the tower is being placed</param>
    internal void Setup(TowerSlot towerSlot, GameManager gameManager)
    {
        this.towerSlot = towerSlot;

        this.gameManager = gameManager;

        // starts scanning for enemies to attack
        StartScanning();
    }

    /// <summary>
    /// Attacks an enemy using the Tower's attack
    /// </summary>
    /// <param name="enemy">The enemy being attacked</param>
    protected virtual void Attack(Enemy enemy)
    {
        // sets the time since last attack to the current moment
        lastAttackTime = DateTime.Now;
    }

    /// <summary>
    /// Scans for enemies, attacking if possible
    /// </summary>
    private void ScanEnemies()
    {
        // if it cant attack
        if (!CanAttack)
        {
            // dont scan
            return;
        }

        // enemies in range of the gun
        List<Enemy> enemies = new List<Enemy>();

        // goes through each enemy
        foreach (Enemy enemy in gameManager.Enemies)
        {
            // gets the distance between the tower and the enemy (Pythagoras)
            float distance = Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2) + Mathf.Pow(enemy.transform.position.y - transform.position.y, 2));

            // if the enemy is in range
            if (distance >= minRange && maxRange >= distance)
            {
                enemies.Add(enemy);
            }
        }

        // if there are no enemies to attack, return
        if (enemies.Count == 0)
        {
            return;
        }

        // gets the enemy farthest down the path

        // sets variables up to count the farthest enemy
        float farthest = 0;
        Enemy attackedEnemy = null;

        // goes through each attackable enemy
        foreach (Enemy enemy in enemies)
        {
            // if its further down the path, set the variables to that one
            if (enemy.DistanceTravelled > farthest)
            {
                farthest = enemy.DistanceTravelled;
                attackedEnemy = enemy;
            }
        }

        // aims the tower
        Aim(attackedEnemy);

        // attacks the enemy
        Attack(attackedEnemy);
    }

    /// <summary>
    /// Starts periodically running the ScanEnemies function
    /// </summary>
    private void StartScanning()
    {
        InvokeRepeating(nameof(ScanEnemies), scanDelay, scanDelay);
    }

    /// <summary>
    /// Stops periodically running the ScanEnemies function
    /// </summary>
    private void StopScanning()
    {
        CancelInvoke(nameof(ScanEnemies));
    }

    /// <summary>
    /// Aims the tower at an enemy
    /// </summary>
    /// <param name="enemy">The enemy to aim at</param>
    protected virtual void Aim(Enemy enemy)
    {
        float angle;

        // JUST SOME TRIG, I SWEAR ITS NOT SCARY

        // arctan of the triangle between the tower and the enemy
        float sine = Mathf.Atan2(Mathf.Abs(enemy.transform.position.y - transform.position.y), Mathf.Abs(enemy.transform.position.x - transform.position.x));

        if (enemy.transform.position.x > transform.position.x)
        {
            // enemy to the right of tower (PI > angle)
            if (enemy.transform.position.y > transform.position.y)
            {
                // enemy beneath tower (PI > angle > PI/2)
                angle = Mathf.PI / 2 + sine;
            }
            else
            {
                // enemy above tower (PI/2 > angle > 0)
                angle = Mathf.PI / 2 - sine;
            }
        }
        else
        {
            // enemy to the left of tower (angle > PI)
            if (enemy.transform.position.y > transform.position.y)
            {
                // enemy beneath tower (3PI/2 > angle > PI)
                angle = 3 * Mathf.PI / 2 - sine;
            }
            else
            {
                // enemy above tower (2PI > angle > 3PI/2)
                angle = 3 * Mathf.PI / 2 + sine;
            }
        }

        // sets the rotation to face the enemy
        transform.rotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);
    }

    /// <summary>
    /// Loads the json data in for the tower
    /// </summary>
    protected void LoadJson()
    {
        // loads in the data for the tower
        data = json.LoadJsonData();

        // levels up the object to level 1 (0) (loads in data)
        LevelUp();

        // loads in the data for the projectile
        projectileData = projectileJson.LoadJsonData();

        // shows that the json files have been loaded
        jsonsLoaded = true;
    }

    /// <summary>
    /// Call when the tower is leveled up
    /// </summary>
    internal virtual void LevelUp()
    {
        // if the variables have been loaded, level up propperly
        if (!jsonsLoaded)
        {
            level++;
        }

        // loads in the variables if they are in the data for this level
        data[level].Load(ref minRange, nameof(minRange));
        data[level].Load(ref maxRange, nameof(maxRange));
        data[level].Load(ref attackDelay, nameof(attackDelay));
    }
}
