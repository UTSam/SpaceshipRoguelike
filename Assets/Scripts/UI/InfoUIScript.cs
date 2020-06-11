using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIScript : MonoBehaviour
{

    public Image spaceshipImage;
    public GameObject spaceship;

    public GameObject activeWeaponUI;
    public GameObject inventorySlotUI;
    public GameObject activeWeaponSpawnPoint;
    public GameObject inventorySlotSpawnPoint;
    public GameObject mainObject;
    public GameObject inventoryElementPrefab;

    public int activeWeaponUISpace = 25;
    public float spaceshipImageSize = 30;

    public int inventorySlotRows = 2;
    public int inventorySlotColumns = 4;

    public List<GameObject> activeWeaponUIList;
    public List<GameObject> inventorySlotList;

    // Pointer on the inventory hud state
    protected GameObject inventoryState;

    // Start is called before the first frame update
    void Start()
    {
        inventoryState = GameObject.Find("Main");

        SetSpaceShipImage();

        SetActiveWeaponUI();
        SetInventorySlotUI();

        SetUIOff(); // Disable the UI at launch

        AddStartingWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        PrintUI();
    }

    public void UpdateWeapons()
    {
        // Free the list of weapons on the spaceship
        spaceship.GetComponent<InventoryScript>().RemoveElements();

        //Add the new list of weapons on the spaceship
        for (int i = 0; i < spaceship.GetComponent<InventoryScript>().weaponNumber; i++)
        {
            if (activeWeaponUIList[i].GetComponent<ActiveWeaponUIScript>().element)
                spaceship.GetComponent<InventoryScript>().SetWeapon(i, activeWeaponUIList[i].GetComponent<ActiveWeaponUIScript>().element.GetComponent<InventoryElementScript>().element);
            else
                spaceship.GetComponent<InventoryScript>().SetWeapon(i, null);
        }

        // Add and place the position of the weapons in the world
        spaceship.GetComponent<InventoryScript>().ActivateWeapons();
    }

    public void SetSpaceShipImage()
    {
        spaceshipImage.sprite = spaceship.GetComponent<SpriteRenderer>().sprite;
        spaceshipImage.transform.localScale = spaceship.GetComponent<Transform>().localScale;

    }

    public void SetActiveWeaponUI()
    {
        float weaponImageWidth = activeWeaponUI.GetComponent<ActiveWeaponUIScript>().weaponImage.GetComponent<RectTransform>().rect.width;
        int nbWeapons = spaceship.GetComponent<InventoryScript>().weaponNumber;

        for (int i=0; i < spaceship.GetComponent<InventoryScript>().weaponNumber; i++)
        {
            GameObject wUI = Instantiate(activeWeaponUI);

            wUI.transform.SetParent(gameObject.transform, false);
            wUI.transform.position = activeWeaponSpawnPoint.transform.position;
            wUI.transform.position = new Vector3(wUI.transform.position.x - (weaponImageWidth * nbWeapons + spaceshipImageSize * (nbWeapons-1))/2 + (weaponImageWidth + spaceshipImageSize) * i + weaponImageWidth*0.5f,
                                                 wUI.transform.position.y,
                                                 wUI.transform.position.z);

            activeWeaponUIList.Add(wUI);
            wUI.transform.SetParent(mainObject.transform);
        }
    }

    public void SetInventorySlotUI()
    {
        float weaponImageWidth = activeWeaponUI.GetComponent<ActiveWeaponUIScript>().weaponImage.GetComponent<RectTransform>().rect.width;
        float weaponImageHeight = activeWeaponUI.GetComponent<ActiveWeaponUIScript>().weaponImage.GetComponent<RectTransform>().rect.height;

        for (int j = 0; j < inventorySlotRows; j++)
        {
            for (int i = 0; i < inventorySlotColumns; i++)
            {
                GameObject wUI = Instantiate(inventorySlotUI);

                wUI.transform.SetParent(gameObject.transform, false);
                wUI.transform.position = inventorySlotSpawnPoint.transform.position;
                wUI.transform.position = new Vector3(wUI.transform.position.x - (weaponImageWidth * inventorySlotColumns + spaceshipImageSize * (inventorySlotColumns - 1)) / 2 + (weaponImageWidth + spaceshipImageSize) * i + weaponImageWidth * 0.5f,
                                                     wUI.transform.position.y + j * weaponImageHeight * 1.5f,
                                                     wUI.transform.position.z);

                inventorySlotList.Add(wUI);
                wUI.transform.SetParent(mainObject.transform);
            }
        }
    }

    public void PrintUI()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryState.GetComponent<InventoryStateScript>().SetState(true);

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            inventoryState.GetComponent<InventoryStateScript>().SetState(false);

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    public void SetUIOff()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void AddWeaponInInventory(GameObject weapon)
    {
        int i = -1;
        bool condition = false;

        do
        {
            i++;
            if(inventorySlotList[i].GetComponent<InventorySlotScript>().element == null)
            {
                // Instantiate and initialize element
                GameObject elem = Instantiate(inventoryElementPrefab);
                elem.GetComponent<InventoryElementScript>().SetElement(weapon);
                elem.transform.SetParent(this.transform);

                //Set the element in the first available inventory slot
                inventorySlotList[i].GetComponent<InventorySlotScript>().SetElement(elem);
                elem.SetActive(false);

                condition = true;
            }

        } while (!condition && i < inventorySlotList.Count);
    }

    public void AddStartingWeapons()
    {
        if (transform.Find("startingWeaponList"))
        {
            
            GameObject list = transform.Find("startingWeaponList").gameObject;

            for (int i=0; i< spaceship.GetComponent<InventoryScript>().weaponNumber; i++)
            {
                if (list.transform.GetChild(0).gameObject.GetComponent<BasicWeapon>())
                {
                    // Instantiate and initialize element
                    GameObject elem = Instantiate(inventoryElementPrefab);
                    GameObject weapon = list.transform.GetChild(0).gameObject;
                    elem.GetComponent<InventoryElementScript>().SetElement(weapon);
                    elem.transform.SetParent(this.transform);



                    //Set the element in the available active weapon slots
                    //activeWeaponUIList[i].GetComponent<ActiveWeaponUIScript>().SetElement(elem);
                    inventorySlotList[i].GetComponent<InventorySlotScript>().SetElement(elem);
                    elem.SetActive(false);

                    //Destroy(weapon);
                }
            }
        }
    }
}
