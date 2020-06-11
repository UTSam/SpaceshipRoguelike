using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    public int weaponNumber = 4;

    public GameObject weaponUI;

    public List<GameObject> weaponList;
    public List<GameObject> weaponPositionList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (weaponList[0]) weaponList[0].GetComponent<BasicWeapon>().Shoot();
        }

        if (Input.GetMouseButton(1))
        {
            Debug.Log("piew");
            weaponList[1].GetComponent<BasicWeapon>().Shoot();
        }
    }

    public void SetWeapon(int index, GameObject weapon)
    {
        weaponList[index] = weapon;
    }

    public void RemoveElements()
    {
        for (int i = 0; i < weaponNumber; i++)
        {
            weaponList[i] = null;
        }
    }

    public void ActivateWeapons()
    {
        for (int i = 0; i < weaponNumber; i++)
        {
            if (weaponList[i])
            {
                weaponList[i].transform.SetParent(this.transform);
                weaponList[i].transform.position = weaponPositionList[i].transform.position;
                weaponList[i].transform.rotation.Set(0f, 0f, 0f, 0f);
                
            }
        }

        // Refresh weapon UI
        weaponUI.GetComponent<WeaponsHandler>().SetWeaponUI();
    }

}
