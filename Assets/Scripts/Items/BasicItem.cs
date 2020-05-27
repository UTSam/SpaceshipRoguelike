using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : BasicEntity
{
    [SerializeField] protected float LifeSpan = 10f;
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (LifeSpan < 0.0f)
            Destroy(this.gameObject);

        LifeSpan -= Time.deltaTime;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.GetComponent<Player>() != null)
            OnPickUp();
    }

    public virtual void OnPickUp()
    {
        Destroy(this.gameObject);
    }
}
