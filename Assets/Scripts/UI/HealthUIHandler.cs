/*
    Authors:
      Robbert Ritsema
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIHandler : MonoBehaviour
{
    private GameObject spaceship;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    [SerializeField] private Image dashBar;
    [SerializeField] private Image ultiBar;

    private void Start()
    {
        spaceship = GVC.Instance.PlayerGO;
    }

    // Update is called once per frame
    void Update()
    {
        if (spaceship == null) return;

        healthBar.fillAmount = spaceship.GetComponent<HealthComponent>().Health / (spaceship.GetComponent<HealthComponent>().MaxHealth);
        shieldBar.fillAmount = spaceship.GetComponent<HealthComponent>().Shield / (spaceship.GetComponent<HealthComponent>().MaxShield);

        //if (spaceship.GetComponent<Player>().movementCouldown == spaceship.GetComponent<Player>().movementCouldownDefault)
        //    dashBar.fillAmount = 1;
        //else
        //    dashBar.fillAmount = (spaceship.GetComponent<Player>().movementCouldownDefault - spaceship.GetComponent<Player>().movementCouldown) / spaceship.GetComponent<Player>().movementCouldownDefault;
    }
}
