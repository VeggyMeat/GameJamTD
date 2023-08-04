using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Tower
{
    [SerializeField] private AudioClip cannonSound;

    protected override void Attack(Enemy enemy)
    { 
        base.Attack(enemy);

        // plays the cannon sound
        AudioSource.PlayClipAtPoint(cannonSound, transform.position);

        // creates the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // gets the projectile script
        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        // sets up the projectile
        projectileComponent.Setup(projectileData[level], (transform.eulerAngles.z + 90) * Mathf.Deg2Rad);
    }
}
