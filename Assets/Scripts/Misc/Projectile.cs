using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Dictionary<string, string> variables;

    protected float lifeSpan;
    protected float velocity;
    protected float damage;

    protected float r;
    protected float g;
    protected float b;

    /// <summary>
    /// Sets up the new projectile
    /// </summary>
    /// <param name="variables">The data for the variables</param>
    /// <param name="angle">Angle in radians of the direction its facing</param>
    public void Setup(Dictionary<string, string> variables, float angle)
    {
        // sets the variables
        this.variables = variables;

        // loads in the variabkes
        variables.Load(ref r, nameof(r));
        variables.Load(ref g, nameof(g));
        variables.Load(ref b, nameof(b));
        variables.Load(ref lifeSpan, nameof(lifeSpan));
        variables.Load(ref velocity, nameof(velocity));
        variables.Load(ref damage, nameof(damage));

        // sets up the colour of the object
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);

        // sets up the movement of the projectile
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * velocity, Mathf.Sin(angle) * velocity);
    }

    /// <summary>
    /// Called to kill the projectile
    /// </summary>
    internal virtual void Die()
    {
        // destroys the gameObject
        Destroy(gameObject);
    }

    internal void OnTriggerEnter2D(Collider2D collision)
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
