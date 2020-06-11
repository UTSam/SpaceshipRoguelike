using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponUIScript : MonoBehaviour
{
    public GameObject element; // The element is of type InventoryElement

    public Image weaponImage;
    public Text weaponText;

    public float weaponImageSize = 10f; // Also in InfoUIScript

    // Start is called before the first frame update
    void Start()
    {
        if (element != null)
            SetElement(element);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetElement(GameObject inventoryElement)
    {
        element = inventoryElement;
        inventoryElement.GetComponent<InventoryElementScript>().SetPosition(weaponImage.transform.position);
        weaponText.text = element.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().GetWeaponData();
        element.transform.SetAsLastSibling();

        element.GetComponent<InventoryElementScript>().SetEnabled(true);
        element.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().SetEquiped(true);

        UpdateWeapons();

        //Change rotation of the weapon
        var weaponRotation = element.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().gameObject.transform.localRotation.eulerAngles;
        weaponRotation.Set(0, 0, 90);
        element.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().gameObject.transform.localRotation = Quaternion.Euler(weaponRotation);
    }

    public void UpdateWeapons()
    {
        // Update spaceship weapon list
        transform.parent.parent.GetComponent<InfoUIScript>().UpdateWeapons();
    }

    public void RemoveElement()
    {
        element = null;
        weaponText.text = string.Empty;
    }
}
