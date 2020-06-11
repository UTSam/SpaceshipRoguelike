/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunWeapon : BasicWeapon
{
    public float laserOverloadFactor = 10;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        shotCooldown = 0;
    }

    public override void Shoot()
    {
        if (shotCooldown <= 0 && !isReloading)
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
}
