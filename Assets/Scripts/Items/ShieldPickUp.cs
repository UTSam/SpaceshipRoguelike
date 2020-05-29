using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : BasicItem
{
    public int RestoreValue = 50;
    public override void OnPickUp()
    {
        HealthComponent health = GVC.Instance.PlayerGO.GetComponent<HealthComponent>();
        if (health.RestoreShield(RestoreValue))
            Destroy(this.gameObject);
    }
}
