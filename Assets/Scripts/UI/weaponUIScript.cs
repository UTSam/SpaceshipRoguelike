/*
    Authors:
      Samuel Boileau
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponUIScript : MonoBehaviour
{
    public GameObject weapon;

    public Text weaponNameTextZone;
    public Text currentBulletNumber;

    public Image bulletsBar;
    public Image weaponImage;
    public Image reloadBar;
    public Image shotCooldownBar;

    public GameObject mainObject;

    private float maxBulletNumber;

    public void Init()
    {
        weaponNameTextZone.text = weapon.GetComponent<BasicWeapon>().weaponName;
        weaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        shotCooldownBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        maxBulletNumber = weapon.GetComponent<BasicWeapon>().magazineSize; // At start the currentBulletNumberisMaxed /!\ Needs to be updated with modifiers

        currentBulletNumber.text = ((int)weapon.GetComponent<BasicWeapon>().currentBulletNumber).ToString();
        bulletsBar.fillAmount = (float)weapon.GetComponent<BasicWeapon>().currentBulletNumber / maxBulletNumber;

        reloadBar.fillAmount = (float)weapon.GetComponent<BasicWeapon>().reloadCooldown / (float)weapon.GetComponent<BasicWeapon>().reloadTime;

        shotCooldownBar.fillAmount = (float)weapon.GetComponent<BasicWeapon>().shotCooldown / (1 / (float)weapon.GetComponent<BasicWeapon>().fireRate);
    }
}
