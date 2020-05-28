using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HealthComponent : MonoBehaviour
{
    [SerializeField] public float Health = 1;
    [SerializeField] public float Shield = 1;

    public float MaxHealth = 100;
    public float MaxShield = 100;

    [SerializeField] private float FireWeakeness = 3f;
    [SerializeField] private float ElecWeakness = 3f;

    public bool isInvincible = false;

   private LifeBar bar; //HealthBar is not required but can be used for visual indications

    protected void Start()
    {
        Health = MaxHealth;
        Shield = MaxShield;
        bar = GetComponentInChildren<LifeBar>();
        UpdateBar();
    }

    public virtual void OnDeath()
    {
        if (GetComponent<Player>())
            SceneManager.LoadScene("DeathScreen");
        else
        {
            if (Random.value < 0.3f)
            {
                GameObject pack = Instantiate(GVC.Instance.HealthPackPrefab) as GameObject;
                pack.transform.position = transform.position;
            }
            Destroy(this.gameObject);
        }
    }

    public virtual void Damage(float damageValue, ElementType elem)
    {
        if (!isInvincible)
        {
            float damageToLife = damageValue;
            if (Shield > 0f)
            {
                if (elem == ElementType.Elec)
                {
                    // Debug.Log("Damage to shield : " + damageValue * (1f + ElecWeakness));
                    Shield -= damageValue * (1f + ElecWeakness);
                }

                else
                {
                    Shield -= damageValue;
                    //Debug.Log("Damage to shield : " + damageValue);
                }
                Shield = Mathf.Round(Shield);

                if (Shield < 0f)
                {
                    if (elem == ElementType.Elec)
                        damageToLife = -Shield / (1f + ElecWeakness);
                    else
                        damageToLife = -Shield;
                    Shield = 0f;
                }
                else
                    damageToLife = 0f;

        if (damageToLife > 0f)
        {
            if (elem == ElementType.Fire)
            {
                Health -= damageToLife * (1f + FireWeakeness);
                //Debug.Log("Damage to life : " + damageToLife * (1f + FireWeakeness));
            }
            else
            {
                Health -= damageToLife;
                //Debug.Log("Damage to life : " + damageToLife);
            }

            if (damageToLife > 0f)
            {
                if (elem == ElementType.Fire)
                {
                    Health -= damageToLife * (1f + FireWeakeness);
                    //Debug.Log("Damage to life : " + damageToLife * (1f + FireWeakeness));
                }
                else
                {
                    Health -= damageToLife;
                    //Debug.Log("Damage to life : " + damageToLife);
                }

                Health = Mathf.Round(Health);
                if (Health <= 0f)
                {
                    OnDeath();
                }
            }

            if (GetComponent<Player>())
                StartCoroutine(TurnInvincible());
            UpdateBar();
        }
        if (GetComponent<Animate>() && GetComponent<Player>() && GetComponent<Player>().GetComponent<Animate>())
        {
            GetComponent<Animate>().DoAnimationOnHit();
        }

        UpdateBar();
    }

    public virtual void Damage(float damageValue)
    {
        if (!isInvincible)
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
                Health -= damageToLife;
                if (Health <= 0f)
                {
                    OnDeath();
                }
            }
            if (GetComponent<Player>())
                StartCoroutine(TurnInvincible());
            UpdateBar();
        }

        if (GetComponent<Animate>() && GetComponent<Player>() && GetComponent<Player>().GetComponent<Animate>())
        {
            GetComponent<Animate>().DoAnimationOnHit();
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
    private IEnumerator TurnInvincible()
    {
        isInvincible = true;
        Color color = this.GetComponent<SpriteRenderer>().color;
        for (int i =0; i< 5; i++)
        {
            //this.GetComponent<SpriteRenderer>().enabled = false;
            color.a   = 0.5f;
            this.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
            //this.GetComponent<SpriteRenderer>().enabled = true;
            color.a = 1f;
            this.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
    }
}
