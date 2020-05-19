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
        bulletSpeed = 6f;
        bulletDamages = 3f;

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
                SetBulletSpeed(-20f,20f);
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            
        }
    }
}
