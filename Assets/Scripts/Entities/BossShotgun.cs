using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgun : BossWeapon
{
    public int BulletsPerShot = 3;
    public int spreadAngle = 45;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        base.Update();
    }

    protected override void FireSequence()
    {
        float angleOffset = 0;
        if (BulletsPerShot > 1)
            angleOffset = spreadAngle * 2f / (BulletsPerShot-1);

        Vector3 lookPos = aimingPosition - Muzzle.position;
        float canonAngle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(canonAngle - 90, Vector3.forward);

        for (int i = 0; i< BulletsPerShot; i++)
        {
            GameObject projectile = Instantiate(ProjectilePrefab) as GameObject;
            projectile.transform.position = Muzzle.position;
            projectile.GetComponent<MovingEntity>().speed = (aimingPosition - Muzzle.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;
            lookPos = aimingPosition - Muzzle.position;
            if (spreadAngle > 0.0f)
                lookPos = Quaternion.AngleAxis(-spreadAngle+i* angleOffset, Vector3.forward) * lookPos;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            projectile.GetComponent<MovingEntity>().speed = lookPos * projectile.GetComponent<BasicProjectile>().InitialSpeed;
        }

        lastShotTimer = 0.0f;
        NbShotToFire--;

        if (NbShotToFire <= 0)
        {
            StartCoroutine(MoveIn());
        }
    }
}
