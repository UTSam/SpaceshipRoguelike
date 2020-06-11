/*
    Authors:
      Thibaut Rousselet
      Jelle van Urk
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : BasicItem
{
    public int HealValue = 50;
    public override void OnPickUp()
    {
        HealthComponent health = GVC.Instance.PlayerGO.GetComponent<HealthComponent>();
        if (health.Heal(HealValue))
        {
            if (GetComponent<Animate>())
            {
                GetComponent<Animate>().DoAnimationSpecial();
            }
            Destroy(this.gameObject);
        }
    }
}
