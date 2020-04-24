using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public Tilemap tilemap_walls;

    // Start is called before the first frame update
    void Start()
    {
        ObjectFromTilemap();
    }

    public void ObjectFromTilemap()
    {
        List<Tile> tiles = new List<Tile>();
        List<Vector3Int> pos = new List<Vector3Int>();

        int minX = int.MinValue, minY = int.MinValue;

        // Add tiles to list
        for (int x = tilemap_walls.cellBounds.xMin; x < tilemap_walls.cellBounds.xMax; x++)
        {
            for (int y = tilemap_walls.cellBounds.yMin; y < tilemap_walls.cellBounds.yMax; y++)
            {
                Tile tile = tilemap_walls.GetTile<Tile>(new Vector3Int(x, y, 0));

                if (tile != null)
                {
                    tiles.Add(tile);
                    pos.Add(new Vector3Int(x, y, 0));

                    Debug.Log(tile.sprite.name);
                }
            }
        }

        // Create empty gameobject, add "Room" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>();
        room.tiles = tiles.ToArray();
        room.positions = pos.ToArray();
        /*room.tilemap_walls = tilemap_walls;
        room.bounds = new BoundsInt();*/
    }
}
