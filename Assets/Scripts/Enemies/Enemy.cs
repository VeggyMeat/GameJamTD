using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;

    private float distanceTravelled;

    private float maxHealth;
    private float health;

    /// <summary>
    /// The distance travelled down the path by the enemy
    /// </summary>
    public float DistanceTravelled
    {
        get
        {
            return distanceTravelled;
        }
    }

    /// <summary>
    /// Called to setup the enemy
    /// </summary>
    /// <param name="gameManager">the gameManager</param>
    public void Setup(GameManager gameManager)
    {
        // sets the gameManager
        this.gameManager = gameManager;
    }

    /// <summary>
    /// Changes the health of the enemy by the value, returning if it survived or not
    /// </summary>
    /// <param name="change">The value to change the health by</param>
    /// <returns>Whether the enemy survived</returns>
    public bool ChangeHealth(float change)
    {
        // changes the health according to the value
        health += change;

        // if it heals too much, reset it down to maxHealth
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // if its health is below 0, kill it
        if (health < 0)
        {
            Die();

            return false;
        }

        return true;
    }

    /// <summary>
    /// Called to kill the enemy
    /// </summary>
    private void Die()
    {

        // kills the gameObject
        Destroy(gameObject);
    }
}
