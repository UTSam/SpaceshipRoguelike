using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMPAvoidance : MonoBehaviour
{
    [SerializeField] Feelers feelers = null;
    public Vector2 WallAvoidance(BasicMovingEnemy host)
    {
        return feelers.CalculateForce(host);
    }
}
