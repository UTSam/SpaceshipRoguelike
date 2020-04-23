using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : BasicEntity
{
    public float MaxSpeed;
    public Vector2 Speed;
    public float Deceleration = 0.0f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public void UpdatePosition()
    {
        Speed -= Speed * Deceleration * Time.deltaTime;
        move(Speed);
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
