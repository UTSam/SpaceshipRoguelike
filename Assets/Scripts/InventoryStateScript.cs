using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStateScript : MonoBehaviour
{
    public bool inventoryOn = false;

    public void SetState(bool state)
    {
        inventoryOn = state;
    }

    public bool GetState()
    {
        return inventoryOn;
    }
}
