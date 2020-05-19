using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    public GameObject spaceship;
    public Image healthBar;
    public Image shieldBar;
    public Image dashBar;
    public Image ultiBar;

    // Update is called once per frame
    void Update()
    {

        healthBar.fillAmount = spaceship.GetComponent<HealthComponent>().Health / ( spaceship.GetComponent<HealthComponent>().MaxHealth * 2 );
        shieldBar.fillAmount = spaceship.GetComponent<HealthComponent>().Shield / ( spaceship.GetComponent<HealthComponent>().MaxShield * 2 );

        if (spaceship.GetComponent<Player>().movementCouldown == spaceship.GetComponent<Player>().movementCouldownDefault)
            dashBar.fillAmount = 1;
        else
            dashBar.fillAmount = (spaceship.GetComponent<Player>().movementCouldownDefault - spaceship.GetComponent<Player>().movementCouldown) / spaceship.GetComponent<Player>().movementCouldownDefault;
    }
}
