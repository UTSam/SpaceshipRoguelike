using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public TileBase[] tiles;
    public Vector3Int[] positions;

    public Tilemap tilemap; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(Vector3Int pos)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap.SetTile(positions[i] + pos, tiles[i]);
        }
    }
}
