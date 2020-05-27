using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthComponent : HealthComponent
{
    private Boss boss;
    [SerializeField] float phase2HPTrigger;
    [SerializeField] float phase3HPTrigger;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        boss = GetComponent<Boss>();
    }

    public override void Damage(float damageValue)
    {
        base.Damage(damageValue);
    }

    public override void Damage(float damageValue, ElementType elem)
    {
        base.Damage(damageValue, elem);

    }

    private void updatePhase()
    {
        if (Health < phase3HPTrigger)
            boss.SetPhase(3);
        else if (Health < phase2HPTrigger)
            boss.SetPhase(3);
    }
}
