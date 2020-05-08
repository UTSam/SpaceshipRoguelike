﻿using System;
using UnityEngine;

public class MovingEntity : BasicEntity
{
    public Vector2 speed;
    public float deceleration = 0.0f;

    public Vector2 heading;
    public Vector2 perpendicular; //Perpendicular vector to the heading vector
    public float mass = 10.0f;     //Weight of an entity                                               ==> default 10
    public float maxSpeed = 20.0f; //Max speed of an entity                                            ==> default 20
    public float maxForce = 2.0f; //Max selfboost of an entity                                         ==> default 2
    public float maxTurnRate = 2.0f; // Max turn rate in radians/s in which an entity can rotate       ==> default 2

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public void UpdatePosition()
    {
        //Speed -= Speed * Deceleration * Time.deltaTime;
        move(speed);
    }

    public void move(Vector2 displacementValue)
    {
        if (displacementValue.magnitude < this.maxSpeed)
        {
            rigidBody.velocity = displacementValue;
        }
        else
        {
            displacementValue.Normalize();
            rigidBody.velocity = displacementValue * this.maxSpeed;
        }
    }

    public void rotate(float rotateValue)
    {
        rigidBody.SetRotation(rigidBody.rotation + rotateValue);
    }

    public Vector2 Perpendicular(Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }

    public Vector2 GetPosition()
    {
        return this.rigidBody.position;
    }

    public float CalculateDistance(MovingEntity targetEntity)
    {
        return (float)Math.Pow((float)Math.Pow((this.transform.position.x - targetEntity.transform.position.x),2) 
            + (float)Math.Pow((this.transform.position.y - targetEntity.transform.position.y), 2) 
            + (float)Math.Pow((this.transform.position.z - targetEntity.transform.position.z), 2),0.5);
    }

    public float CalculateDistance(Vector2 vector2)
    {
        return (float)Math.Sqrt(
            (float)Math.Pow((this.transform.position.x - vector2.x), 2)
            + (float)Math.Pow((this.transform.position.y - vector2.y), 2));
    }
}