using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int Health = 10;

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public virtual void Damage(int damageValue)
    {
        Health -= damageValue;
    }

    public virtual void Heal(int damageValue)
    {
        Health -= damageValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            OnDeath();
        }
    }
}
