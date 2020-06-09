using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWaveRifle : BossWeapon
{
    public int spreadAngle = 30;
    public int DefaultNbWaveToFire = 1;
    private int NbWaveToFire = 0;
    private bool isLeftToRight = true;
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        NbWaveToFire = DefaultNbWaveToFire;
}

    protected void Update()
    {
        base.Update();
    }

    public override void StopShooting()
    {
        base.StopShooting();
        NbWaveToFire = DefaultNbWaveToFire;
    }

    // Update is called once per frame
    protected override void FireSequence()
    {
        float angleOffset = 0;
        angleOffset = spreadAngle * 2f / (DefaultNbShotToFire - 1);

       
        /*    Vector3 lookPos = aimingPosition - Muzzle.position;
        float canonAngle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(canonAngle - 90, Vector3.forward);*/

        GameObject projectile = Instantiate(ProjectilePrefab) as GameObject;
        projectile.transform.position = Muzzle.position;
        projectile.GetComponent<MovingEntity>().speed = (aimingPosition - Muzzle.position).normalized * projectile.GetComponent<BasicProjectile>().InitialSpeed;

        Vector3 lookPos = aimingPosition - Muzzle.position;

        if (isLeftToRight)
            lookPos = Quaternion.AngleAxis(-spreadAngle + (DefaultNbShotToFire - NbShotToFire) * angleOffset, Vector3.forward) * lookPos;
        else
            lookPos = Quaternion.AngleAxis(spreadAngle-(angleOffset/2f) - (DefaultNbShotToFire - NbShotToFire) * angleOffset, Vector3.forward) * lookPos;

        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        projectile.GetComponent<MovingEntity>().speed = lookPos * projectile.GetComponent<BasicProjectile>().InitialSpeed;

        lastShotTimer = 0.0f;
        NbShotToFire--;
        if (NbShotToFire <= 0)
        {
            NbWaveToFire--;
            if (NbWaveToFire > 0)
            {
                isLeftToRight = !isLeftToRight;
                NbShotToFire = DefaultNbShotToFire;
            }
            else
            {
                NbWaveToFire = DefaultNbWaveToFire;
                StartCoroutine(MoveIn());
            }
        }
    }
}