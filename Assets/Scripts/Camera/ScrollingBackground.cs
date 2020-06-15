/*
    Authors:
      Robbert Ritsema
*/
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ScrollingBackground : MonoBehaviour
{
    public float backgroundSpeed;
    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() 
    {
        Vector2 cameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        objectRenderer.material.mainTextureOffset = cameraPosition / backgroundSpeed;
    }
}
