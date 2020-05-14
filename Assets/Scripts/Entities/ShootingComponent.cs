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

    private LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()

        if (Target != null && lastShotTimer >= 1.0f/FireRate && !Physics2D.Linecast(Muzzle.position, Target.position, mask))
        {
            GameObject projectile = Instantiate(ProjectilePrefab) as GameObject;
            projectile.transform.position = Muzzle.position;
            projectile.GetComponent<MovingEntity>().speed = (Target.position - Muzzle.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;

            
            Vector3 lookPos = Target.position - Muzzle.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            lastShotTimer = 0.0f;
        }
        lastShotTimer += Time.deltaTime;
    }
}
