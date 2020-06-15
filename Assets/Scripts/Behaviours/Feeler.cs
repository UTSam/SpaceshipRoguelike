/*
    Authors:
      Thibaut Rousselet
*/

using UnityEngine;
using UnityEngine.Tilemaps;

public class Feeler : MonoBehaviour
{
    public bool IsCollidingWall = false;
    public Transform Top;
    public Transform Origin;

    public float MaxValue = 1f;

    protected void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.GetComponent<TilemapCollider2D>())
        {
            IsCollidingWall = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {

        if (other.GetComponent<TilemapCollider2D>())
        {
            IsCollidingWall = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Origin.position, Top.position);
    }

    public Vector2 GetOppositeDirection()
    {
        return -(Top.position - Origin.position).normalized;
    }
}
