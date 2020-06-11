/*
    Authors:
      Thibaut Rousselet
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldInventory : MonoBehaviour
{
    public int Golds = 0;
    private GoldUI UI;

    private static GoldInventory instance = null;
    public static GoldInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoldInventory>();
            }
            return instance;
        }
    }

    private void Start()
    {
        UI = GetComponentInChildren<GoldUI>();
    }
    public void AddGold(int value)
    {
        Golds += value;
        UI.UpdateDisplay(Golds);
    }

    public bool Pay(int price)
    {
        if (Golds >= price)
        {
            Golds -= price;
            UI.UpdateDisplay(Golds);
            return true;
        }
        else
        {
            return false;
        }
    }
}
