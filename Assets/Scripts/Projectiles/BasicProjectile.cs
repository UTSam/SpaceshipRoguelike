using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasicProjectile : MovingEntity
{
    public int DamageValue = 1;
    public float LifeSpan = 3.0f;
    public float InitialSpeed = 10.0f;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        LifeSpan -= Time.deltaTime;

        if (LifeSpan <= 0.0f)
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
        } if (other.GetComponentInParent<TilemapCollider2D>())
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }
}