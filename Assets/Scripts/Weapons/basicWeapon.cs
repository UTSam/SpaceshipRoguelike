using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicWeapon : MonoBehaviour
{
    public enum EffectsList { fire, poison, electric, explosive, penetrate, bounce, multiply };

    // Bullet attributes
    private Vector2 direction; // direction is normalized 
    public float bulletSize = 1f;
    public float bulletSpeed = 2f;
    public float bulletDamages = 10f;
    public List<EffectsList> effectsList;

    // Weapon attribute
    public string riffleName = "Riffle";
    public int magazineSize = 5;
    private int currentBulletNumber;
    public int reloadTime = 2; // in seconds
    public float fireRate = 2f;
    public int maxModifierNumber = 3;

    // Bullet object and spawn point
    public GameObject bullet;
    public Transform bulletSpawnPoint;

    // Time variables
    private float reloadCouldown = 0;
    private float shotCouldown = 0;

    // Triger variables
    private bool isReloading = false;


    private Quaternion rotation;
    private Vector3 localPosition;

    // Start is called before the first frame update
    void Start()
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
    void Update()
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

    public void ShootFunction()
    {
        if (Input.GetMouseButton(0) && shotCouldown <= 0 && !isReloading)
        {
            shotCouldown = 1 / fireRate;
            currentBulletNumber--;
            SetBulletSpeed();
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation); 
        }
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

    public void SetBulletSpeed()
    {
        Vector2 direction = Vector2.zero;
        direction.Set(transform.parent.up.x, transform.parent.up.y);
        bullet.GetComponent<MovingEntity>().speed = direction * bulletSpeed;

        //bullet.GetComponent<MovingEntity>().speed =  Vector2.up * bulletSpeed;
    }

    public void SetBullet()
    {
    }
}
