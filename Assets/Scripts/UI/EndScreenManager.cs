using TMPro;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerDisplay;
    [SerializeField] TextMeshProUGUI seedDisplay;
    void Start()
    {
        timerDisplay.text = "Timer : " + GlobalValues.Timer;
        seedDisplay.text = "Seed : " + GlobalValues.Seed;
    }
}
