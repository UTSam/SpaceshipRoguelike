﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingEntity
{
    void Start()
    {
        
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        move(new Vector2(horizontalInput, verticalInput)*5); 
    }
}
