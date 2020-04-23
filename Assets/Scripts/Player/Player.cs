using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingEntity
{
    public override void Start()
    {
        base.Start();
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Speed = new Vector2(horizontalInput*10.0f, verticalInput*10.0f);
        UpdatePosition();
    }

    private void OnDestroy()
    {
        
    }
}
