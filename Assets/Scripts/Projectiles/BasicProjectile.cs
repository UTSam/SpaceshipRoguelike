using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MovingEntity
{
    public int DamageValue;
    public float LifeSpan;
    private float lifeTime;


    // Start is called before the first frame update
    void Start()
    {
        lifeTime = LifeSpan;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime = LifeSpan - Time.time;
        if (lifeTime <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void OnHit(Collider2D other)
    {
        if (other.GetComponentInParent<Player>())
        {
            other.GetComponentInParent<HealthComponent>().Damage(DamageValue);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }
}