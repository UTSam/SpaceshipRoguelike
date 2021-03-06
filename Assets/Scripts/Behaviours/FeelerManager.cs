﻿/*
    Authors:
      Thibaut Rousselet
*/
using UnityEngine;

//Handle the feelers by checking their collision and return the corresponding force
public class FeelerManager : MonoBehaviour
{
    [SerializeField] private float force = 2f;
    private LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Wall");
    }
    public Vector2 CalculateForce()
    {
        Vector2 returnValue = Vector2.zero;
        foreach (Feeler feel in GetComponentsInChildren<Feeler>())
        {
            if (feel.IsCollidingWall)
            {
                RaycastHit2D hit = Physics2D.Linecast(feel.Origin.position, feel.Top.position, mask);
                if (hit.collider != null)
                {
                    //Get an opposite force based on position of volision along the feeler
                    returnValue += feel.GetOppositeDirection()* force * (1-hit.fraction); 
                }
            }
        }
        return returnValue;
    }
}
