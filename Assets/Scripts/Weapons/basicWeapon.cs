/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    // Weapon attribute
    public string weaponName;
    public int magazineSize;
    public float currentBulletNumber;
    public int reloadTime; // in seconds
    public float fireRate;
    public int maxModifierNumber;

    // Bullet object and spawn point
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public Transform bulletDirection;

    // Time variables
    public float reloadCooldown = 0;
    public float shotCooldown = 0;

    // Triger variables
    protected bool isReloading = false;

    [SerializeField] protected AudioSource fireSound;

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentBulletNumber = magazineSize;
    }

    // Update is called once per frame
    public void Update()
    {
        Shoot();

        // Reloads if the number of bullets is null or if the user ask for it, only if it is not already reloading or if its bullet number is max
        Reload();

        /* Timer updates */
        if (shotCooldown > 0)
            shotCooldown -= Time.deltaTime;

        if (reloadCooldown > 0)
            reloadCooldown -= Time.deltaTime;
        /* End timer updates */
    }

    public virtual void Shoot() {}

    public void Reload()
    {
        if (isReloading && reloadCooldown <= 0)
        {
            isReloading = false;
            currentBulletNumber = magazineSize;
        }

        if (((Input.GetKeyDown(KeyCode.R) && (currentBulletNumber < magazineSize)) || currentBulletNumber == 0) && reloadCooldown <= 0)
        {
            isReloading = true;
            reloadCooldown = reloadTime;
        }
    }
}
