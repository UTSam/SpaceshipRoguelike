using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public List<BasicWeapon> weaponList;
    public GameObject weaponUI;
    public Camera camera;

    public int uiSpace = 50;

    // Start is called before the first frame update
    void Start()
    {

        for (int i=0; i<weaponList.Count; i++)
        {
            GameObject wUI = Instantiate(weaponUI);
            wUI.transform.SetParent(gameObject.transform, false);
            wUI.GetComponent<weaponUIScript>().weapon = weaponList[i];
            wUI.GetComponent<weaponUIScript>().Start();
            wUI.GetComponent<Canvas>().worldCamera = camera;
            wUI.GetComponent<weaponUIScript>().mainObject.transform.localPosition = new Vector3(0, wUI.transform.position.y + uiSpace * i, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
