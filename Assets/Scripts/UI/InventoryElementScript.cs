using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElementScript : MonoBehaviour
{
    public GameObject element;
    public Image imageElement;
    public GameObject mainObject;

    // Start is called before the first frame update
    void Start()
    {
        // Called if the element is set on the launch of the program
        if (element != null)
        {
            element = Instantiate(element);
            element.transform.SetParent(transform);
            SetElement(element);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetElement(GameObject elem)
    {
        element = elem;
        imageElement.sprite = element.GetComponent<SpriteRenderer>().sprite;
        element.transform.SetParent(transform);
    }

    public void SetEnabled(bool state)
    {
        element.SetActive(state);
    }

    public void SetVisible(bool state) // An element can only be visible when dragged
    {
        mainObject.SetActive(state);
    }

    internal void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
