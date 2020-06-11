/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BasicWeapon
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
        if (Input.GetMouseButton(0) && shotCooldown <= 0 && !isReloading)
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
