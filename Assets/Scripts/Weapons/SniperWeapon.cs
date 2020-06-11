﻿/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperWeapon : BasicWeapon
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        if (shotCooldown <= 0 && !isReloading)
        {
            shotCooldown = 1 / fireRate;
            currentBulletNumber--;

            FireProjectile();
        }
    }


    private void FireProjectile()
    {
        GameObject projectile = Instantiate(bullet) as GameObject;
        projectile.transform.position = bulletSpawnPoint.position;
        projectile.GetComponent<MovingEntity>().speed = (bulletDirection.position - bulletSpawnPoint.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;

        Vector3 lookPos = bulletDirection.position - bulletSpawnPoint.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
