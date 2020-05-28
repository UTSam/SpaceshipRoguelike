using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunWeapon : BasicWeapon
{
    public override void Start()
    {
        // Bullet attributes
        bulletSize = 0.5f;
        bulletSpeed = 25f;
        bulletDamages = 25f;
        bulletLifeSpan = 3f;

        // Weapon attribute
        weaponName = "Machine Gun";
        magazineSize = 50;
        reloadTime = 8; // in seconds
        fireRate = 5f;
        maxModifierNumber = 1;

        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void ShootFunction()
    {
        if (Input.GetMouseButton(0) && shotCooldown <= 0 && !isReloading)
        {
            shotCooldown = 1 / fireRate;
            currentBulletNumber--;
            //SetBulletSpeed(0f,0f);
            /*GameObject bulletGO = Instantiate(bullet, bulletSpawnPoint.position);//, bulletSpawnPoint.rotation);
            bulletGO.GetComponent<MovingEntity>().speed = (bulletDirection.position - bulletSpawnPoint.position).normalized * bulletGO.GetComponent<BasicProjectile>().InitialSpeed;*/

            GameObject projectile = Instantiate(bullet) as GameObject;
            projectile.transform.position = bulletSpawnPoint.position;
            projectile.GetComponent<MovingEntity>().speed = (bulletDirection.position - bulletSpawnPoint.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;


            Vector3 lookPos = bulletDirection.position - bulletSpawnPoint.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            projectile.SetActive(true);
        }
    }
}
