using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Boolean followPlayer;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GVC.Instance.PlayerGO.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        if(followPlayer)
            transform.position = GetPosition();
    }

    internal Vector3 GetPosition()
    {
        if (!followPlayer)
            return transform.position;

        Vector3 playerPosition = playerTransform.position;
        playerPosition.z = 0;

        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);
        cursorPosition.z = 0;

        Vector3 middlePoint = Vector3.Lerp(playerPosition, cursorPosition, 0.3f);

        middlePoint.z = -60;
        return middlePoint;
    }
}