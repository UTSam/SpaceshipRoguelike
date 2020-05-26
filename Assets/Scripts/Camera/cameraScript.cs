using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GVC.Instance.PlayerGO.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 jacco = playerTransform.position;
        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);

        Vector3 middlePoint = Vector3.Lerp(jacco, cursorPosition, 0.3f);
        middlePoint.z = -60;

        transform.position = middlePoint;
    }
}