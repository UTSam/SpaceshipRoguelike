using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiffleWeapon : BasicWeapon
{



    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
        // Bullet attributes
        bulletSize = 1f;
        bulletSpeed = 15f;
        bulletDamages = 10f;

        // Weapon attribute
        weaponName = "RiggleGun";
        magazineSize = 15;
        reloadTime = 2; // in seconds
        fireRate = 2f;
        maxModifierNumber = 3;
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
            SetBulletSpeed(0f,0f);
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}
