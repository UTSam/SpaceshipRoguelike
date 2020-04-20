using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = new PlayerMovement(rb);
    }
    void Update()
    {
        playerMovement.Update();
    }

    void FixedUpdate()
    {
        playerMovement.FixedUpdate();
    }
}
