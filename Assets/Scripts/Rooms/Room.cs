using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Tile[] tiles;
    public Vector3Int[] positions;

    public Tilemap tilemap; 

    // Start is called before the first frame update
    void Awake()
    {
        tilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(Vector3Int pos)
    {
        if(tilemap == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap.SetTile(positions[i] + pos, tiles[i]);
        }
    }

    public List<Vector3Int> GetDoorPositions()
    {
        List<Vector3Int> tempPos = new List<Vector3Int>();
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].sprite.name == "Door")
                tempPos.Add(positions[i]);
        }

        return tempPos;
    }
}
