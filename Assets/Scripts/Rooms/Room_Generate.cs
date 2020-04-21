using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room_Generate : MonoBehaviour
{
    public BoundsInt area;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        ObjectFromTilemap(tilemap);
    }

    private void ObjectFromTilemap(Tilemap tilemap)
    {
        List<TileBase> tiles = new List<TileBase>();
        List<Vector3Int> pos = new List<Vector3Int>();

        // Add tiles to list
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Tile tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));

                if (tile != null)
                {
                    tiles.Add(tile);
                    pos.Add(new Vector3Int(x, y, 0));

                    Debug.Log(tile.sprite.name);
                }
            }
        }

        // Create empty gameobject, add "Tile" script and populate values
        GameObject newObj = new GameObject("Room");
        newObj.AddComponent<Room>();
        newObj.GetComponent<Room>().tiles = tiles.ToArray();
        newObj.GetComponent<Room>().positions = pos.ToArray();
        newObj.GetComponent<Room>().tilemap = tilemap;

        tilemap.ClearAllTiles();

        newObj.GetComponent<Room>().CreateRoom(new Vector3Int(0,0,0));
    } 
}
