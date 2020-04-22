using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : BasicEnemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        rotate(1.0f);
    }
}
