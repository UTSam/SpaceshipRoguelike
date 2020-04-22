using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : BasicEntity
{
    public float MaxSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void move(Vector2 displacementValue)
    {
        if (displacementValue.magnitude < MaxSpeed)
            RigidBody.velocity = displacementValue;
        else
        {
            displacementValue.Normalize();
            RigidBody.velocity = displacementValue * MaxSpeed;
        }
    }

    public void rotate(float rotateValue)
    {
        RigidBody.SetRotation(RigidBody.rotation + rotateValue);
    }

}
