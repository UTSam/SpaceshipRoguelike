using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    public GameObject cameraCenter;
    private Quaternion rotation;
    private Vector3 localPosition;

    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = rotation;
        this.transform.position = transform.root.position + localPosition * transform.root.localScale.y;
    }
}
