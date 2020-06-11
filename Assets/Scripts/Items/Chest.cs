using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float pickupRange;
    [SerializeField] private List<GameObject> possibleLoot;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(this.transform.position, GVC.Instance.PlayerGO.transform.position) > pickupRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        System.Random rnd = new System.Random();
        GameObject item = possibleLoot[rnd.Next(possibleLoot.Count)];

        Vector3 spawnPosition = transform.position;
        spawnPosition.y -= .5f;

        Instantiate(item, spawnPosition, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
