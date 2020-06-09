using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : ShootingComponent
{
    [SerializeField]protected int NbShotToFire = 0; //Nb shot fire per suqeuence
    [SerializeField]public int DefaultNbShotToFire = 0; //Nb shot fire per suqeuence
    protected Vector3 aimingPosition; //Last position of the target
    protected Boss boss;
    public bool IsAiming;
    public bool IsRotating = false;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        boss = transform.root.GetComponent<Boss>();
    }

    protected void Update()
    {
        if (Target != null && lastShotTimer >= 1.0f / FireRate && NbShotToFire > 0)
        {
            FireSequence();
        }
        lastShotTimer += Time.deltaTime;
        if (IsAiming || NbShotToFire <=0)
        {
            Vector3 lookPos = Target.position - Muzzle.position;
            float canonAngle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(canonAngle - 90, Vector3.forward);
        }
    }

    public virtual void StopShooting()
    {
        NbShotToFire = 0;
        StartCoroutine(MoveIn());
    }

    protected virtual void FireSequence()
    {
        lastShotTimer = 0.0f;
        NbShotToFire--;
    }

    public void InitFireSequence()
    {
        boss.IsReadyToFIre = false;
        StartCoroutine(MoveOut());
    }

    private IEnumerator MoveOut()
    {
        while(transform.localPosition.y > -1.0f)
        {
            transform.position += new Vector3(0f, -0.05f, 0f);
            yield return null;
        }
        NbShotToFire = DefaultNbShotToFire;
        aimingPosition = Target.position;
    }

    public IEnumerator MoveIn()
    {
        while (transform.localPosition.y < 0.0f)
        {
            transform.position += new Vector3(0f, 0.05f, 0f);
            yield return null;
        }
        boss.IsReadyToFIre = true;
    }
}