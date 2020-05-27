using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : BasicWeapon
{
    int bulletsPerShot;

    // Start is called before the first frame update
    public override void Start()
    {
        // Bullet attributes
        bulletSize = 1f;
        bulletSpeed = 13f;
        bulletDamages = 50f;
        // Weapon attribute
        weaponName = "Shotgun";
        magazineSize = 5;
        reloadTime = 3; // in seconds
        fireRate = 0.8f;
        maxModifierNumber = 3;
        bulletsPerShot = 10;

        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        base.Update();
        setLifeSpan(1.0f);
    }

    public override void ShootFunction()
    {
        if (Input.GetMouseButton(0) && shotCooldown <= 0 && !isReloading)
        {
            shotCooldown = 1 / fireRate;
            currentBulletNumber--;

            for(int i = 0; i < bulletsPerShot; i++)
            {
                //GameObject bulletGO = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                //SetBulletSpeed(bulletGO.transform, -30f, 30f);
                //bulletGO.GetComponent<MovingEntity>().speed = (bulletDirection.position - bulletSpawnPoint.position).normalized * bulletGO.GetComponent<BasicProjectile>().InitialSpeed;
                //Vector2 noise = AddNoiseOnAngle(-30f, 30f).normalized;
                /*Vector3 direction = bulletDirection.position - bulletSpawnPoint.position;
                direction = Quaternion.AngleAxis(Random.Range(-10f,10f), Vector3.back) * direction;
                bulletGO.GetComponent<MovingEntity>().speed = direction.normalized * bulletGO.GetComponent<BasicProjectile>().InitialSpeed* Random.Range(0.8f, 1.2f);*/

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
}
