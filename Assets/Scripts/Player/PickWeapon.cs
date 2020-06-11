using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    public GameObject infoUI;
    public float maxPickDistance = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        infoUI = GameObject.Find("InventoryUI");
    }

    // Update is called once per frame
    void Update()
    {
        //Grabs all the weapons on the floor with the Key E
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, maxPickDistance);
            int i = 0;
            while (i < hitColliders.Length)
            {
                // If the element hit is a weapon we hide it and add it in the inventory
                if (hitColliders[i].transform.gameObject.GetComponent<BasicWeapon>() && Vector3.Distance(hitColliders[i].transform.position, this.transform.position) < maxPickDistance)
                {
                    // Equip only if the weapon is not equiped
                    if (!hitColliders[i].transform.gameObject.GetComponent<BasicWeapon>().equiped)
                    {
                        infoUI.GetComponent<InfoUIScript>().AddWeaponInInventory(hitColliders[i].transform.gameObject);
                    }
                }

                i++;
            }
        }

        // Grabs the pointed weapon on the floor with the central mouse button
        if (Input.GetMouseButtonDown(2))
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
