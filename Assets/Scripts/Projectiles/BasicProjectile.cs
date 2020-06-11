/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ElementType
{
    Fire,
    Elec,
    None
}

public class BasicProjectile : MovingEntity
{
    public float DamageValue = 10f;
    public float LifeSpan = 3.0f;
    public float InitialSpeed = 20.0f;
    public ElementType element = ElementType.None;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
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
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            player.GotHit(DamageValue, element);
            FireAnimations();
            Destroy(this.gameObject);
        }

        if (other.GetComponentInParent<TilemapCollider2D>())
        {
            FireAnimations();
            Destroy(this.gameObject);
        }
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }

    private void FireAnimations()
    {
        if (GetComponent<Animate>())
        {
            GetComponent<Animate>().DoAnimationOnHit();
            if (GetComponent<SFX_Player>())
            {
                GetComponent<SFX_Player>().PlayOnImpact();
            }
        }

    }
}