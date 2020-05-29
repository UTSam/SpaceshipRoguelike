using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LifeBar : MonoBehaviour
{
    [SerializeField] GameObject healthBar = null;
    [SerializeField] GameObject shieldBar = null;
    [SerializeField] bool IsFollowingEntity = true;

    //private Quaternion rotation;
    //private Vector3 localPosition;

    void Start()
    {
        //rotation = transform.root.rotation;
        //this.transform.localPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (IsFollowingEntity)
        {
            transform.rotation = Quaternion.identity;
            this.transform.position = GetComponentInParent<HealthComponent>().transform.position - new Vector3(0, 1.5f, 0);//this.transform.localPosition * transform.root.localScale.y;
        }
    }

    public void SetHealthValue(float value)
    {
        healthBar.transform.localScale = new Vector3(value, 1);
    }

    public void SetShieldValue(float value)
    {
        shieldBar.transform.localScale = new Vector3(value, 1);
    }
}
