/*
    Authors:
      Samuel Boileau
*/

using UnityEngine;

public class ShotgunWeapon : BasicWeapon
{
    int bulletsPerShot = 10;

    public override void Shoot()
    {
        if (shotCooldown <= 0 && !isReloading)
        {
            shotCooldown = 1 / fireRate;
            currentBulletNumber--;

            if (fireSound) fireSound.Play();
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject projectile = Instantiate(bullet) as GameObject;
            projectile.transform.position = bulletSpawnPoint.position;
            Vector3 direction = (bulletDirection.position - bulletSpawnPoint.position).normalized;
            direction = Quaternion.AngleAxis(Random.Range(-10f, 10f), Vector3.forward) * direction;
            projectile.GetComponent<MovingEntity>().speed = direction * projectile.GetComponent<BasicProjectile>().InitialSpeed * Random.Range(0.8f, 1.2f); ;


            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
