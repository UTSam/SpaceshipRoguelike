/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    private GameObject spaceship;
    public List<GameObject> weaponList;
    public GameObject weaponUI;
    public Camera camera;
    public List<GameObject> weaponUIList;

    public int uiSpace = 50;

    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        spaceship = GVC.Instance.PlayerGO;
        weaponList = spaceship.GetComponent<InventoryScript>().weaponList;
        SetWeaponUI();
    }

    public void SetWeaponUI()
    {
        //Delete current elements
        for (int i = 0; i < weaponUIList.Count; i++)
        {
            Destroy(weaponUIList[i]);
        }
        weaponUIList.Clear();

        // Add the elements
        int j = 0; // Variable j used to avoid holes in the UI
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i] != null)
            {
                GameObject wUI = Instantiate(weaponUI);
                wUI.transform.SetParent(gameObject.transform, false);
                wUI.GetComponent<weaponUIScript>().weapon = weaponList[i];
                wUI.GetComponent<weaponUIScript>().Init();
                wUI.GetComponent<Canvas>().worldCamera = camera;
                wUI.GetComponent<weaponUIScript>().mainObject.transform.localPosition += new Vector3(0, uiSpace * j, 0);
                weaponUIList.Add(wUI);

                j++;
            }

        }
    }
}
