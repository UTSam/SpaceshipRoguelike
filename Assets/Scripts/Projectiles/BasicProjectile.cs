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
    public float InitialSpeed = 10.0f;
    public ElementType element = ElementType.None;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (GetComponent<SFX_Player>())
        {
            GetComponent<SFX_Player>().PlayShootingSFX();
        }
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
        if (other.GetComponentInParent<Player>())
        {
            other.GetComponentInParent<HealthComponent>().Damage(DamageValue, element);
            MyAnimation();
            Destroy(this.gameObject);

        }

        if (other.GetComponentInParent<TilemapCollider2D>())
        {
            MyAnimation();
            Destroy(this.gameObject);
        }


    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
    }

    private void MyAnimation()
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

    //private void OnDestroy()
    //{
    //    GameObject newObj = new GameObject("projectileImpact");
    //    SFX_Player sfxplayer = newObj.AddComponent<SFX_Player>();
    //    sfxplayer.OnDeathSound(GetComponent<SFX_Player>()));
    //    Instantiate(newObj);

    //    GetComponent<Animate>().DoAnimationOnHit();
    //    if (GetComponent<SFX_Player>())
    //    {
    //        GetComponent<SFX_Player>().PlayOnDeath();

    //    }
    //}
}