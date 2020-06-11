/*
    Authors:
      Robbert Ritsema
*/

using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Boolean followPlayer;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        playerTransform = GVC.Instance.PlayerGO.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform == null) return;

        if (followPlayer)
            transform.position = GetPosition();
    }

    internal Vector3 GetPosition()
    {
        if (!followPlayer)
            return transform.position;

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 middlePoint = Vector3.Lerp(playerTransform.position, cursorPosition, 0.3f);

        middlePoint.z = -60;
        return middlePoint;
    }
}