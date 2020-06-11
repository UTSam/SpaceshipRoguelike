/*
    Authors:
      Jelle van Urk
      Robbert Ritsema
*/

using UnityEngine;

public class BossSquareLauncher : BossShotgun
{
    [SerializeField]private float maxSpread = 30f;
    private bool isSpreading = true;
    protected override void FireSequence()
    {
        if (spreadAngle < maxSpread && isSpreading)
            spreadAngle += maxSpread / DefaultNbShotToFire*2;
        else
        {
            isSpreading = false;
            spreadAngle -= maxSpread / DefaultNbShotToFire*2;
        }
        base.FireSequence();
        if (NbShotToFire <= 0)
            isSpreading = true;
    }

    public override void StopShooting()
    {
        base.StopShooting();
        isSpreading = true;
    }
}
