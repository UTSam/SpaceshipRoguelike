using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BasicEntity : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected Collider2D bodyCollider;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float DotProduct(Vector2 a, Vector2 b)
    {
        float returnvalue = (a.x * b.x) + (a.y * b.y);

        return returnvalue;
    }
}
