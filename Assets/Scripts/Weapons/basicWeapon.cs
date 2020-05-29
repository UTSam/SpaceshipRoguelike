using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public enum EffectsList { explosive, penetrate, bounce, multiply };
    public enum ElementType { Fire, Elec, None };

    // Bullet attributes
    public float bulletSize;
    public float bulletSpeed;
    public float bulletDamages;
    public float bulletLifeSpan;
    public List<EffectsList> effectsList;

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

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentBulletNumber = magazineSize;

        SetBulletValues();
    }

    // Update is called once per frame
    public void Update()
    {
        ShootFunction();

        // Reloads if the number of bullets is null or if the user ask for it, only if it is not already reloading or if its bullet number is max
        ReloadFunction();

        /* Timer updates */
        if (shotCooldown > 0)
            shotCooldown -= Time.deltaTime;

        if (reloadCooldown > 0)
            reloadCooldown -= Time.deltaTime;
        /* End timer updates */
    }

    public virtual void ShootFunction()
    {
    }

    public void ReloadFunction()
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

    public void SetBulletSpeed(Transform transform, float min, float max)
    // float min and max represents the limit angles that can be added to the original spaceship orientation
    {
        Vector2 direction = Vector2.zero;
        direction.Set(transform.parent.up.x, transform.parent.up.y);
        direction += AddNoiseOnAngle(min, max);
        bullet.GetComponent<MovingEntity>().speed = direction.normalized * bulletSpeed;
    }

    public virtual void SetBulletValues()
    {
        bullet.GetComponent<BasicProjectile>().LifeSpan = bulletLifeSpan;
        bullet.GetComponent<Transform>().localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        bullet.GetComponent<BasicProjectile>().DamageValue = bulletDamages;
        bullet.GetComponent<BasicProjectile>().InitialSpeed = bulletSpeed;
    }

    protected Vector2 AddNoiseOnAngle(float min, float max)
    // float min and max represents the limit angles that can be added to the original spaceship orientation
    {
        float xNoise = Random.Range(min, max);
        float yNoise = Random.Range(min, max);
        // Now get the angle between w.r.t. a vector 2 direction
        Vector2 noise = new Vector3(
            Mathf.Sin(2f * 3.1415926f * xNoise / 360),
            Mathf.Sin(2f * 3.1415926f * yNoise / 360)
                        );
        return noise;
    }

    protected void SetLifeSpan(float newValue)
    {
        bullet.GetComponent<BasicProjectile>().LifeSpan = newValue;
    }
}
