/*
    Authors:
      Samuel Boileau
*/

using UnityEngine;

public class Pistol : BasicWeapon
{
    public override void Shoot()
    {
        if (shotCooldown <= 0 && !isReloading)
        {
            shotCooldown = 1 / fireRate;
            currentBulletNumber--;

            FireProjectile();
            if (fireSound) fireSound.Play();
        }
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(bullet) as GameObject;
        Vector3 lookPos = bulletDirection.position - bulletSpawnPoint.position;

        projectile.transform.position = bulletSpawnPoint.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        projectile.GetComponent<BasicProjectile>().speed = lookPos * projectile.GetComponent<BasicProjectile>().InitialSpeed;
    }
}
