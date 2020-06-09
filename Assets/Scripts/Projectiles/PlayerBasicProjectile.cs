/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicProjectile : BasicProjectile
{

    public enum EffectsList {explosive, penetrate, bounce, multiply };

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
        if (!other.GetComponentInParent<Player>() && !other.GetComponentInParent<BasicProjectile>() && !other.GetComponent<BasicItem>())
        {
            if (other.GetComponentInParent<HealthComponent>())
            {
                other.GetComponentInParent<HealthComponent>().Damage(DamageValue, element);
                if (GetComponent<Animate>())
                {
                    GetComponent<Animate>().DoAnimationOnHit();
                }
            }
            Destroy(this.gameObject);
        }
    }
}