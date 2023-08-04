using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MortarProjectile : Projectile
{
    private float range;
    [SerializeField] private AudioClip explosionSound;

    internal override void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile colides with an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // explode play audio
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            // gets all the objects within the range
            Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, range);

            // gets all of the enemies within the range
            Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

            foreach (Collider2D enemy in enemiesInCircle)
            {
                // if its an enemy
                if (enemy.gameObject.tag == "Enemy")
                {
                    // grab the script
                    Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();

                    // deal damage to the enemy
                    enemyScript.ChangeHealth(-damage);
                }
            }

            base.OnTriggerEnter2D(collision);
        }
    }

    public override void Setup(Dictionary<string, string> variables, float angle)
    {
        base.Setup(variables, angle);

        // loads in the range variable
        variables.Load(ref range, nameof(range));
    }
}
