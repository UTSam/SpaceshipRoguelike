using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicProjectile : BasicProjectile
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void OnHit(Collider2D other)
    {
        if (!other.GetComponentInParent<Player>() && !other.GetComponentInParent<BasicProjectile>())
        {
            if (other.GetComponentInParent<HealthComponent>())
            {
                other.GetComponentInParent<HealthComponent>().Damage(DamageValue, element);
                
            }
            Destroy(this.gameObject);
            Debug.Log(other);
        }
    }
}
