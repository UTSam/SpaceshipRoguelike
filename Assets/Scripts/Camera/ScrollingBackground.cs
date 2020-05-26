using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSpeed;
    private Renderer renderer;
    public bool AutoScroll = false;
    public float scrollingSpeed = 250;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (AutoScroll)
        {
            renderer.material.mainTextureOffset += new Vector2(0, 1)/scrollingSpeed;
        } else
        {
            Vector2 cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            renderer.material.mainTextureOffset = cameraPosition / backgroundSpeed;
        }
    }
}
