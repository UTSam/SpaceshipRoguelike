/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public LineRenderer lr;

    public float laserLength = 10;

    public float DamageValue;
    public ElementType element = ElementType.None;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, transform.position - transform.up * 10);

        lr.SetPosition(1, transform.position - transform.up * laserLength);

        if (hit.collider != null)
        {
            if (!hit.collider.gameObject.GetComponentInParent<Player>() && !hit.collider.gameObject.GetComponentInParent<BasicProjectile>() && !hit.collider.gameObject.GetComponent<PlayerBasicProjectile>())
            {
                lr.SetPosition(1, hit.point);

                if (hit.collider.gameObject.GetComponentInParent<HealthComponent>())
                {
                    hit.collider.gameObject.GetComponentInParent<HealthComponent>().Damage(DamageValue, element);

                    if (GetComponent<Animate>())
                    {
                        GetComponent<Animate>().DoAnimationOnHit();
                    }
                }
            }
        }
    }
}
