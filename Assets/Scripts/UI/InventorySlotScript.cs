using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public GameObject element; // The element is of type InventoryElement

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
        inventoryElement.GetComponent<InventoryElementScript>().SetPosition(transform.position);

        //Disable the weapon in the world
        element.GetComponent<InventoryElementScript>().SetEnabled(false);

        element.transform.SetAsLastSibling();
    }

    public void RemoveElement()
    {
        element = null;
    }
}
