using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : BasicEntity
{
    private bool pickedUp = false;
    public override void Start()
    {
        base.Start();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;

        if (other.transform.root.GetComponent<Player>() != null)
        {
            OnPickUp(other.transform.root.gameObject);
            Destroy(this.gameObject);
            pickedUp = true;
        }

    }

    public virtual void OnPickUp(GameObject other)
    {
        Debug.Log("Item picked up");
        Destroy(this.gameObject);
    }
}
