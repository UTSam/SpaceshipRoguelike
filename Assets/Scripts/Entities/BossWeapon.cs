using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : ShootingComponent
{
    [SerializeField]protected int NbShotToFire = 0; //Nb shot fire per suqeuence
    [SerializeField]public int DefaultNbShotToFire = 0; //Nb shot fire per suqeuence
    protected Vector3 aimingPosition; //Last position of the target

    protected bool movingFront = false;
    protected bool movingBack = false;
    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (Target != null && lastShotTimer >= 1.0f / FireRate && NbShotToFire > 0)
        {
            FireSequence();
        }
        lastShotTimer += Time.deltaTime;
    }
    // Update is called once per frame
    protected virtual void FireSequence()
    {
        lastShotTimer = 0.0f;
        NbShotToFire--;
    }

    public void InitFireSequence()
    {
        NbShotToFire = DefaultNbShotToFire;
        aimingPosition = Target.position;
    }
}