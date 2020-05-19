using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicCamera : MonoBehaviour
{
    public GameObject player;
    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 jacco = player.transform.position;
        jacco.z = -60;
        transform.position = jacco;
    }
}
