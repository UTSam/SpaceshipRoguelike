using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : BasicItem
{
    public int HealValue = 10;
    public override void OnPickUp(GameObject playerGO)
    {
        HealthComponent health = playerGO.GetComponent<HealthComponent>();
        if (health.Heal(HealValue))
            Destroy(this.gameObject);
    }
}
