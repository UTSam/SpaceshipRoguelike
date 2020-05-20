using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerDisplay;
    [SerializeField] TextMeshProUGUI seedDisplay;
    void Start()
    {
        timerDisplay.text = "Timer : " + GlobalValues.Timer.Remove(GlobalValues.Timer.LastIndexOf('.'));
        seedDisplay.text = "Seed : " + GlobalValues.Seed;
    }
}
