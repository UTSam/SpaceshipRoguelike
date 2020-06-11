/*
    Authors:
      Thibaut Rousselet
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : BossWeapon
{
    [SerializeField] GameObject EnemyTospawn;

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
        Instantiate(EnemyTospawn, Muzzle.position, Quaternion.identity);
        lastShotTimer = 0.0f;
        NbShotToFire--;

        Vector3 lookPos = aimingPosition - Muzzle.position;
        float canonAngle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(canonAngle - 90, Vector3.forward);

        if (NbShotToFire <= 0)
        {
            StartCoroutine(MoveIn());
        }
    }
}
