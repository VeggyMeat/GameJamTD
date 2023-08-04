using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Tower
{
    protected override void Attack(Enemy enemy)
    {
        // calls the base attack
        base.Attack(enemy);

        // creates the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // gets the projectile script
        Projectile projectileComponent = projectile.GetComponent<Projectile>();

        // sets up the projectile, adds a random angle variation
        projectileComponent.Setup(projectileData[level], (transform.eulerAngles.z + 90) * Mathf.Deg2Rad + Random.Range(-Mathf.PI / 16, Mathf.PI / 16));
    }
}
