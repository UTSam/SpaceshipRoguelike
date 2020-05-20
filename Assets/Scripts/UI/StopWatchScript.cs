using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StopWatchScript : MonoBehaviour
{

    private float timer = 0f;
    public Text textZone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        String timerString = TimeSpan.FromSeconds(timer).ToString();

        if (timerString.LastIndexOf('.') > 0)
        {
            timerString = timerString.Remove(timerString.LastIndexOf('.') + 1);
        }

        textZone.text = timerString.Remove(timerString.Length - 1);
    }
}
