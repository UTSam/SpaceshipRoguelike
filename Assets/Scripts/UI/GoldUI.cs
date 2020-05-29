using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateDisplay(0);
    }

    public void UpdateDisplay(int value)
    {
        text.text = "Gold : "+value;
    }
}
