﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to store static value that you can call from any scene
public static class GlobalValues
{
    public static int Seed = 0;
    public static float TimerValue = 0f;
    public static string Timer;
    public static int Golds = 0;
    public static float health = float.MaxValue;
    public static float shield = float.MaxValue;
}