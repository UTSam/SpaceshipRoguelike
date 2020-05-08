using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LifeBar : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject shieldBar;

    private Quaternion rotation;
    private Vector3 localPosition;
    void Awake()
    {
        rotation = transform.rotation;
        localPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
        this.transform.position = transform.root.position + localPosition * transform.root.localScale.y;
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
