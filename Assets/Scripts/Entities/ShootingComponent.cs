using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingComponent : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform Target;
    public Transform Muzzle;
    [Range(0.0f, 100.0f)]
    public float FireRate = 2.0f;
    private float lastShotTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShotTimer >= 1.0f/FireRate)
        {
            GameObject projectile = Instantiate(ProjectilePrefab) as GameObject;
            projectile.transform.position = Muzzle.position;
            projectile.GetComponent<MovingEntity>().speed = (Target.position - Muzzle.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;

            Debug.DrawLine(Muzzle.position, Target.position, Color.red, 10f);

            Vector3 lookPos = Target.position - Muzzle.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            lastShotTimer = 0.0f;
        }
        lastShotTimer += Time.deltaTime;
    }
}
