using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : Tower
{
    [SerializeField] private AudioClip mortarSound;

    protected override void Attack(Enemy enemy)
    {
        // calls the base attack
        base.Attack(enemy);
        
        // plays the mortar sound
        AudioSource.PlayClipAtPoint(mortarSound, transform.position);

        // creates the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, 180));

        // gets the projectile script
        MortarProjectile projectileComponent = projectile.GetComponent<MortarProjectile>();

        // sets up the projectile
        projectileComponent.Setup(projectileData[level], (transform.eulerAngles.z + 90) * Mathf.Deg2Rad);
    }
}
