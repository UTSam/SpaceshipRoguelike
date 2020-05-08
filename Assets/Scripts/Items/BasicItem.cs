﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : BasicEntity
{
    protected void Start()
    {
        base.Start();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.GetComponent<Player>() != null)
            OnPickUp(other.transform.root.gameObject);
    }

    public virtual void OnPickUp(GameObject other)
    {
        Debug.Log("Item picked up");
        Destroy(this.gameObject);
    }
}