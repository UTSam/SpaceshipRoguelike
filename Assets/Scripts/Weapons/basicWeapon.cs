using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public enum EffectsList { fire, poison, electric, explosive, penetrate, bounce, multiply };

    // Bullet attributes
    protected Vector2 direction; // direction is normalized 
    public float bulletSize;
    public float bulletSpeed;
    public float bulletDamages;
    public List<EffectsList> effectsList;

    // Weapon attribute
    public string weaponName;
    public int magazineSize;
    protected int currentBulletNumber;
    public int reloadTime; // in seconds
    public float fireRate;
    public int maxModifierNumber;

    // Bullet object and spawn point
    public GameObject bullet;
    public Transform bulletSpawnPoint;

    // Time variables
    protected float reloadCouldown = 0;
    protected float shotCouldown = 0;

    // Triger variables
    protected bool isReloading = false;


    protected Quaternion rotation;
    protected Vector3 localPosition;

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentBulletNumber = magazineSize;
        //transform.localPosition = transform.parent.position;
    }

    void Awake()
    {
        rotation = transform.rotation;
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    public void Update()
    {
        //OrientWeapon();


        ShootFunction();
        
        // Reloads if the number of bullets is null or if the user ask for it, only if it is not already reloading or if its bullet number is max
        ReloadFunction();

        /* Timer updates */
        if (shotCouldown > 0)
            shotCouldown -= Time.deltaTime;

        if (reloadCouldown > 0)
            reloadCouldown -= Time.deltaTime;
        /* End timer updates */
    }

    public virtual void ShootFunction()
    {
/*        if (Input.GetMouseButton(0) && shotCouldown <= 0 && !isReloading)
        {
            shotCouldown = 1 / fireRate;
            currentBulletNumber--;
            SetBulletSpeed();
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation); 
        }*/
    }

    public void ReloadFunction()
    {
        if (isReloading && reloadCouldown <= 0)
        {
            isReloading = false;
            currentBulletNumber = magazineSize;
        }

        if (((Input.GetKeyDown(KeyCode.R) && (currentBulletNumber < magazineSize)) || currentBulletNumber == 0) && reloadCouldown <= 0)
        {
            isReloading = true;
            reloadCouldown = reloadTime;
        }
    }

    public void SetBulletSpeed(float min, float max)
    {
        Vector2 direction = Vector2.zero;
        direction.Set(transform.parent.up.x, transform.parent.up.y);
        direction += AddNoiseOnAngle(min, max);
        bullet.GetComponent<MovingEntity>().speed = direction.normalized * bulletSpeed;
    }

    public void SetBullet()
    {
    }

    Vector2 AddNoiseOnAngle(float min, float max)
    {
        float xNoise = Random.Range(min, max);
        float yNoise = Random.Range(min, max);
        // Now get the angle between w.r.t. a vector 3 direction
        Vector2 noise = new Vector3(
            Mathf.Sin(2f * 3.1415926f * xNoise / 360),
            Mathf.Sin(2f * 3.1415926f * yNoise / 360)
                        );
        return noise;
    }
}
