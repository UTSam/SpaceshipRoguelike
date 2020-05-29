﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Patrick : BasicItem
{
    public override void OnPickUp()
    {
        //GlobalValues.Timer = GVC.Instance.StopWatchGO.GetComponentInChildren<Text>().text;
        GlobalValues.TimerValue = GVC.Instance.StopWatchGO.GetComponent<StopWatchScript>().timer;
        SceneManager.LoadScene("BossScene");
    }
}
