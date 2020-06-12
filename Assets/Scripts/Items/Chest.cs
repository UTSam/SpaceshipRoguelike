using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject eButton;
    [SerializeField] private ParticleSystem pwettyParticles;
    [SerializeField] private float pickupRange;
    [SerializeField] private List<GameObject> possibleLoot;

    private AudioSource sound;
    private bool chestOpened = false;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(this.transform.position, GVC.Instance.PlayerGO.transform.position) > pickupRange)
        {
            if (eButton.activeSelf)
                eButton.SetActive(false);

            return;
        }

        if(!eButton.activeSelf)
            eButton.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E) && !chestOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        chestOpened = true;

        sound.Play();
        pwettyParticles.Play();
        Invoke("DestroyChest", sound.clip.length);
    }

    private void DestroyChest()
    {
        SpawnLoot();
        Destroy(this.gameObject);
    }

    private void SpawnLoot()
    {
        System.Random rnd = new System.Random();
        GameObject item = possibleLoot[rnd.Next(possibleLoot.Count)];

        Vector3 spawnPosition = transform.position;
        spawnPosition.y -= .5f;

        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
