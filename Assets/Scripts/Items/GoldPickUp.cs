using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickUp : BasicItem
{
    public int GoldValue = 50;
    public override void OnPickUp()
    {
        GoldInventory.Instance.AddGold(GoldValue);
        Destroy(this.gameObject);
    }
}
