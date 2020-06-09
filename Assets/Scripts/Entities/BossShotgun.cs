/*
    Authors:
      Thibaut Rousselet
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotgun : BossWeapon
{
    public int BulletsPerShot = 3;
    public float spreadAngle = 45;

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
        if (IsAiming)
            lookPos = Target.position - Muzzle.position;

        if (IsRotating)
            lookPos += new Vector3(0.5f, 0f, 0f) * (DefaultNbShotToFire - NbShotToFire);
        Vector3 aimingPos;

        for (int i = 0; i< BulletsPerShot; i++)
        {
            GameObject projectile = Instantiate(ProjectilePrefab) as GameObject;
            projectile.transform.position = Muzzle.position;
            projectile.GetComponent<MovingEntity>().speed = (lookPos).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;
            aimingPos = lookPos;
            if (spreadAngle > 0.0f)
                aimingPos = Quaternion.AngleAxis(-spreadAngle+i* angleOffset, Vector3.forward) * aimingPos;
            float angle = Mathf.Atan2(aimingPos.y, aimingPos.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            projectile.GetComponent<MovingEntity>().speed = aimingPos * projectile.GetComponent<BasicProjectile>().InitialSpeed;
        }

        lastShotTimer = 0.0f;
        NbShotToFire--;

        if (NbShotToFire <= 0)
        {
            StartCoroutine(MoveIn());
        }
    }
}
