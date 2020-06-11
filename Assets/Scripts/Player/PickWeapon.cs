using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    public GameObject infoUI;
    public float maxPickDistance = 2;

    // Start is called before the first frame update
    void Start()
    {
        infoUI = GameObject.Find("InventoryUI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // If the element hit is a weapon we hide it and add it in the inventory
                if (hit.transform.gameObject.GetComponent<BasicWeapon>() && Vector3.Distance(hit.transform.position, this.transform.position) < maxPickDistance)
                {
                    // Equip only if the weapon is not equiped
                    if (!hit.transform.gameObject.GetComponent<BasicWeapon>().equiped)
                    {
                        infoUI.GetComponent<InfoUIScript>().AddWeaponInInventory(hit.transform.gameObject);
                    }
                }
            }
        }
    }
}
