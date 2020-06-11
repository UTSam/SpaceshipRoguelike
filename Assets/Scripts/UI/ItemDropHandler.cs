using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    RectTransform invPanel;
    private GameObject draggedObject;
    private GameObject draggedObjectContainer;
    private GameObject draggedObjectReceiver;
    private bool onElement;

    public GameObject mainObject;

    public GameObject spaceship;

    public void OnDrop(PointerEventData eventData)
    {
        onElement = false;
        draggedObjectContainer = null;
        draggedObjectReceiver = null;

        // Test if the mouse is positioned on the mainObject of the UI
        invPanel = mainObject.transform as RectTransform;
        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            // If mouse not positioned on the mainObject of the UI, delete the element and drop the weapon on the ground
            DestroyDraggedObject();
        } else
        {
            // Check if the mouse is on one of the element of the UI
            foreach (Transform child in mainObject.transform)
            {

                /* Active Weapon UI Test */
                if (child.GetComponent<ActiveWeaponUIScript>())
                {
                    invPanel = child.GetChild(0) as RectTransform;

                    DraggedElementCheck(child.gameObject);

                    if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
                    {
                        onElement = true;
                        draggedObjectReceiver = child.gameObject;
                    }
                }
                /* Inventory Slot Script Test */
                else if (child.GetComponent<InventorySlotScript>())
                {
                    invPanel = child.GetChild(0) as RectTransform;

                    DraggedElementCheck(child.gameObject);

                    if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
                    {
                        onElement = true;
                        draggedObjectReceiver = child.gameObject;
                    }
                }
            }

            // Apply changes if on element
            if (onElement)
            {

                /* Active Weapon UI Test */
                if (draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>())
                {

                    if (draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>().element == null) // there is no element on the receiving container
                    {
                        draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>().SetElement(draggedObject);

                        if (draggedObjectContainer != null)
                        {

                            if (draggedObjectContainer.GetComponent<ActiveWeaponUIScript>())
                            {
                                draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().RemoveElement();
                                draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().UpdateWeapons();
                            }
                            else
                                draggedObjectContainer.GetComponent<InventorySlotScript>().RemoveElement();
                        }
                    }
                    else // there is already an element on the receiving container -> SWAP
                    {
                        if (draggedObjectContainer.GetComponent<ActiveWeaponUIScript>())
                            draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().SetElement(draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>().element);
                        else
                            draggedObjectContainer.GetComponent<InventorySlotScript>().SetElement(draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>().element);

                        draggedObjectReceiver.GetComponent<ActiveWeaponUIScript>().SetElement(draggedObject);
                    }
                }
                /* Inventory Slot Script Test */
                else if (draggedObjectReceiver.GetComponent<InventorySlotScript>())
                {
                    if (draggedObjectReceiver.GetComponent<InventorySlotScript>().element == null) // there is no element on the receiving container
                    {
                        draggedObjectReceiver.GetComponent<InventorySlotScript>().SetElement(draggedObject);

                        if (draggedObjectContainer != null)
                        {
                            if (draggedObjectContainer.GetComponent<InventorySlotScript>())
                                draggedObjectContainer.GetComponent<InventorySlotScript>().RemoveElement();
                            else
                            {
                                draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().RemoveElement();
                                draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().UpdateWeapons();
                            }
                        }
                    }
                    else // there is already an element on the receiving container -> SWAP
                    {
                        if (draggedObjectContainer.GetComponent<InventorySlotScript>())
                            draggedObjectContainer.GetComponent<InventorySlotScript>().SetElement(draggedObjectReceiver.GetComponent<InventorySlotScript>().element);
                        else
                            draggedObjectContainer.GetComponent<ActiveWeaponUIScript>().SetElement(draggedObjectReceiver.GetComponent<InventorySlotScript>().element);

                        draggedObjectReceiver.GetComponent<InventorySlotScript>().SetElement(draggedObject);
                    }
                }

            }

            draggedObject.GetComponent<ItemDragHandler>().SetOnElement(onElement);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        draggedObject = null;
        draggedObjectContainer = null;

        spaceship = GameObject.Find("spaceship");
    }

    public void SetDraggedObject(GameObject obj)
    {
        draggedObject = obj;
    }

    public void DraggedElementCheck(GameObject child) // Check if the current child is contains the dragged object and set draggedObjectContiner if true
    {
        if (child.GetComponent<ActiveWeaponUIScript>())
        {
            if (child.GetComponent<ActiveWeaponUIScript>().element == draggedObject)
                draggedObjectContainer = child;
        } 
        else
        {
            if (child.GetComponent<InventorySlotScript>().element == draggedObject)
                draggedObjectContainer = child;
        }
    }

    public void DestroyDraggedObject()
    {
        draggedObject.GetComponent<InventoryElementScript>().element.SetActive(true);
        draggedObject.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().equiped = false;
        draggedObject.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().transform.position = spaceship.transform.position;
        draggedObject.GetComponent<InventoryElementScript>().element.transform.SetParent(this.transform.parent);

        foreach (Transform child in mainObject.transform)
        {
            /* Active Weapon UI Test */
            if (child.GetComponent<ActiveWeaponUIScript>())
            {
                if(child.GetComponent<ActiveWeaponUIScript>().element == draggedObject)
                {
                    child.GetComponent<ActiveWeaponUIScript>().RemoveElement();
                    Destroy(draggedObject);
                    child.GetComponent<ActiveWeaponUIScript>().UpdateWeapons();
                    break;
                }
            }
            /* Inventory Slot Script Test */
            else if (child.GetComponent<InventorySlotScript>())
            {
                if (child.GetComponent<InventorySlotScript>().element == draggedObject)
                {
                    child.GetComponent<InventorySlotScript>().RemoveElement();
                    draggedObject.GetComponent<InventoryElementScript>().element.GetComponent<BasicWeapon>().transform.position = spaceship.transform.position;
                    Destroy(draggedObject);
                    break;
                }
            }
        }

        
    }
}
