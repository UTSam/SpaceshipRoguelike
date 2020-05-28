using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthComponent : HealthComponent
{
    private Boss boss;
    [SerializeField] float phase2HPTrigger = 0.9f;
    [SerializeField] float phase3HPTrigger = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        boss = GetComponent<Boss>();
    }

    public override void Damage(float damageValue)
    {
        base.Damage(damageValue);
        updatePhase();
    }

    public override void Damage(float damageValue, ElementType elem)
    {
        base.Damage(damageValue, elem);
        updatePhase();
    }

    private void updatePhase()
    {
        if (Health/MaxHealth < phase3HPTrigger)
            boss.SetPhase(3);
        else if (Health / MaxHealth < phase2HPTrigger)
            boss.SetPhase(2);
    }
}
