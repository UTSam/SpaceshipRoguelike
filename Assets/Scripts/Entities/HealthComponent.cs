using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float Health = 1;
    [SerializeField] private float Shield = 1;

    public float MaxHealth = 100;
    public float MaxShield = 100;

    [SerializeField] private LifeBar bar;

    void Start()
    {
        Health = MaxHealth;
        Shield = MaxShield;
        UpdateBar();
    }
    public virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public virtual void Damage(float damageValue)
    {
        float damageToLife = damageValue;
        if (Shield > 0f)
        {
            Shield -= damageValue;
            if (Shield < 0f)
            {
                damageToLife = -Shield;
                Shield = 0f;
            }
            else
                damageToLife = 0f;
        }

        if (damageToLife > 0f)
        {
            Health -= damageValue;
            if (Health <= 0f)
            {
                OnDeath();
            }
        }
        UpdateBar();
    }

    public virtual bool Heal(int healValue)
    {
        if (Health < MaxHealth)
        {
            Health += healValue;
            if (Health > MaxHealth)
                Health = MaxHealth;

            UpdateBar();
            return true;
        } else
            return false;        
    }

    public virtual bool RestoreShield(int restoreValue)
    {
        if (Shield < MaxShield)
        {
            Shield += restoreValue;
            if (Shield > MaxShield)
                Shield = MaxShield;

            UpdateBar();
            return true;
        }
        else
            return false;
    }

    // Start is called before the first frame update


    private void UpdateBar()
    {
        if (bar != null)
        {
            bar.SetHealthValue(Health / MaxHealth);
            if (MaxShield > 0f)
                bar.SetShieldValue(Shield / MaxShield);
            else
                bar.SetShieldValue(0f);
        }
    }
}
