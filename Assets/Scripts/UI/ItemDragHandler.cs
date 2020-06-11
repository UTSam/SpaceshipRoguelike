using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private bool onElement;
    private Vector3 position;

    public void OnBeginDrag(PointerEventData eventData)
    {
        onElement = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        transform.parent.GetComponent<ItemDropHandler>().SetDraggedObject(this.transform.gameObject);

        //Disable the weapon in the world
        this.transform.gameObject.GetComponent<InventoryElementScript>().SetEnabled(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!onElement) // If item dropped on a element of the UI
            transform.localPosition = position;
        else // If item is not dropped on a element of the UI but on the UI
            position = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        position = transform.localPosition;
        onElement = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnElement(bool value)
    {
        onElement = value;
    }


}
