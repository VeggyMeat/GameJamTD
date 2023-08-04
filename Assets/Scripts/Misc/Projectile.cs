using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Dictionary<string, string> variables;

    protected float lifeSpan;
    protected float velocity;
    protected float damage;

    /// <summary>
    /// Sets up the new projectile
    /// </summary>
    /// <param name="variables">The data for the variables</param>
    /// <param name="angle">Angle in radians of the direction its facing</param>
    public virtual void Setup(Dictionary<string, string> variables, float angle)
    {
        // sets the variables
        this.variables = variables;

        // loads in the variabkes
        variables.Load(ref lifeSpan, nameof(lifeSpan));
        variables.Load(ref velocity, nameof(velocity));
        variables.Load(ref damage, nameof(damage));

        // sets up the movement of the projectile
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * velocity, Mathf.Sin(angle) * velocity);

        // kills the component after the lifespan
        Invoke(nameof(Die), lifeSpan);
    }

    /// <summary>
    /// Called to kill the projectile
    /// </summary>
    internal virtual void Die()
    {
        // destroys the gameObject
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when the enemy collides with an object
    /// </summary>
    /// <param name="collision">The object collided with</param>
    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile colides with an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // gets the enemy's script
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // deals that damage to the enemy
            enemy.ChangeHealth(-damage);

            // kills the projectile
            Die();
        }
        // otherwise ignore the collision
    }
}
