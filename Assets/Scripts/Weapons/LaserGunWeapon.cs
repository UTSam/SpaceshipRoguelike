using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunWeapon : BasicWeapon
{

    public float laserOverloadFactor = 10;
    public override void Start()
    {
        // Bullet attributes
        bulletSize = 0.5f;
        bulletSpeed = 15 ;
        bulletDamages = 2f;
        bulletLifeSpan = 30f;

        // Weapon attribute
        weaponName = "Laser Gun";
        magazineSize = 500;
        reloadTime = 2; // in seconds
        fireRate = 20f; // No fire rate with laser, fireRate represents the quantity of bullets that is used at each frames. Increasing the value reduces the speed the gun overload
        maxModifierNumber = 2;

        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        base.Update();
        shotCooldown = 0;
    }

    public override void ShootFunction()
    {
        if (Input.GetMouseButton(0) && shotCooldown <= 0 && !isReloading)
        {
            bullet.SetActive(true);
            currentBulletNumber -= (laserOverloadFactor/fireRate);
        } 
        else if(currentBulletNumber < magazineSize && !isReloading)
        {
            currentBulletNumber += (laserOverloadFactor * 2 /fireRate);
            bullet.SetActive(false);
        } else
        {
            bullet.SetActive(false);
        }
    }

    public override void SetBulletValues()
    {
        bullet.GetComponent<LaserProjectile>().DamageValue = bulletDamages;
        bullet.GetComponent<LineRenderer>().widthMultiplier = bulletSize;
        bullet.GetComponent<LaserProjectile>().laserLength = bulletLifeSpan;
    }
}
