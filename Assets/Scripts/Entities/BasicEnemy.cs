﻿/*
    Authors:
      Thibaut Rousselet
      Jelle van Urk
*/
public class BasicEnemy : MovingEntity  {

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
}
