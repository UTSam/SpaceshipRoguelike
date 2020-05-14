using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Feelers : MonoBehaviour
{
    [SerializeField] private float force = 2f;
    private LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Wall");
    }
    public Vector2 CalculateForce(BasicMovingEnemy host)
    {
        Vector2 returnValue = Vector2.zero;
        foreach (Feeler feel in GetComponentsInChildren<Feeler>())
        {
            if (feel.IsCollidingWall)
            {
                RaycastHit2D hit = Physics2D.Linecast(feel.Origin.position, feel.Top.position, mask);
                if (hit.collider != null)
                {
                    returnValue += feel.GetOppositeDirection()* force * (1-hit.fraction);
                    Debug.DrawLine(feel.Origin.position, feel.Origin.position + (feel.Top.position - feel.Origin.position) * hit.fraction, Color.blue, 10f);
                }
            }
        }
        return returnValue;
    }
}
