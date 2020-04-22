using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider2D))]
public class BasicEntity : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected Collider2D collider;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
