using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : BasicEntity
{
    public float MaxSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(Vector2 displacementValue)
    {
        if (displacementValue.magnitude < MaxSpeed)
        {
            rigidBody.velocity = displacementValue;
        }
        else
        {
            displacementValue.Normalize();
            rigidBody.velocity = displacementValue * MaxSpeed;
        }
    }

    public void rotate(float rotateValue)
    {
        rigidBody.SetRotation(rigidBody.rotation + rotateValue);
    }
}
