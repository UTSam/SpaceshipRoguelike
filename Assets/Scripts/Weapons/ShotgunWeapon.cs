using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : BasicWeapon
{
    int bulletsPerShot;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Bullet attributes
        bulletSize = 1f;
        bulletSpeed = 6f;
        bulletDamages = 10f;

        // Weapon attribute
        weaponName = "ShotGun";
        magazineSize = 5;
        reloadTime = 3; // in seconds
        fireRate = 2f;
        maxModifierNumber = 3;
        bulletsPerShot = 10;
    }


    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void ShootFunction()
    {
        if (Input.GetMouseButton(0) && shotCouldown <= 0 && !isReloading)
        {
            shotCouldown = 1 / fireRate;
            currentBulletNumber--;

            for(int i = 0; i < bulletsPerShot; i++)
            {
                SetBulletSpeed(-20f,20f);
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            
        }
    }
}
