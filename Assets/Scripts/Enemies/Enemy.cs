using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private float maxHealth;
    private float health;
    [SerializeField] private float speed;

    [SerializeField] private AudioClip deathSound;

    public bool isDead = false;

    /// <summary>
    /// The speed of the enemy
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }
    }

    [SerializeField] private int moneyDrop;
    [SerializeField] private int damage;
    private float startingPoint;

    /// <summary>
    /// The distance travelled down the path by the enemy
    /// </summary>
    public float DistanceTravelled
    {
        get
        {
            return transform.position.x - startingPoint;
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

        // sets HP to maxHP
        health = maxHealth;

        // adds the enemy to the list from the gameManager to be tracked
        gameManager.Enemies.Add(this);

        // sets the velocity of the enemy (walking to the right)
        GetComponent<Rigidbody2D>().velocity = -Vector2.left * speed;

        // marks the starting point
        startingPoint = transform.position.x;
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
        // gives money to the player for killing the enemy
        gameManager.Money += moneyDrop;

        // removes the enemy from the gameManager's list
        gameManager.Enemies.Remove(this);

        // kills the gameObject
        Destroy(gameObject);

        // sets the object as dead
        isDead = true;

        // plays the death sound
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    /// <summary>
    /// Called when the enemy collides with an object
    /// </summary>
    /// <param name="collision">The object collided with</param>
    internal void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile colides with the endZone
        if (collision.gameObject.tag == "endZone")
        {
            // damages the player
            gameManager.Health -= damage;

            // kills the enemy
            Die();
        }
        // otherwise ignore the collision
    }
}
