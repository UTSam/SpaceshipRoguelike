﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StopWatchScript : MonoBehaviour
{

    public float timer = 0f;
    public Text textZone;

    // Start is called before the first frame update
    void Start()
    {
        timer = GlobalValues.TimerValue;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        textZone.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }
}
