using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponUIScript : MonoBehaviour
{
    public BasicWeapon weapon;

    public Text weaponNameTextZone;
    public Text currentBulletNumber;

    public Image bulletsBar;
    public Image weaponImage;
    public Image reloadBar;
    public Image shotCooldownBar;

    public GameObject mainObject;

    private float maxBulletNumber;

    //modifiers ...


    // Start is called before the first frame update
    public void Start()
    {
        
    }

    public void Init()
    {
        weaponNameTextZone.text = weapon.weaponName;
        weaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        shotCooldownBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        maxBulletNumber = weapon.magazineSize; // At start the currentBulletNumberisMaxed /!\ Needs to be updated with modifiers

        currentBulletNumber.text = weapon.currentBulletNumber.ToString();
        bulletsBar.fillAmount = (float)weapon.currentBulletNumber / maxBulletNumber;

        reloadBar.fillAmount = (float)weapon.reloadCooldown / (float)weapon.reloadTime;

        shotCooldownBar.fillAmount = (float)weapon.shotCooldown / (1 / (float)weapon.fireRate);
    }
}
