using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : BasicEnemy
{
    public Vector2 Direction;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        move(Direction);
    }
}
