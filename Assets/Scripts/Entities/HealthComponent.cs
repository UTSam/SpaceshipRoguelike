using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]private int Health = 1;
    public int MaxHealth = 100;
    public Text HealthText;

    public virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public virtual void Damage(int damageValue)
    {
        if (Health > 0)
        {
            Health -= damageValue;
            if (Health <= 0)
            {
                OnDeath();
            }
            UpdateText();
        }
    }

    public virtual bool Heal(int healValue)
    {
        if (Health < MaxHealth)
        {
            Health += healValue;
            if (Health > MaxHealth)
                Health = MaxHealth;

            UpdateText();
            return true;
        } else
            return false;        
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        
    }

    private void UpdateText()
    {
        //HealthText.text = Health.ToString() + " / " + MaxHealth.ToString();
    }
}
