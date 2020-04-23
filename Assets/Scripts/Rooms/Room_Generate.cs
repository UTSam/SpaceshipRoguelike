using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room_Generate : MonoBehaviour
{
    public BoundsInt area;
    public Tilemap tilemap;

    private int roomMinX = 9999;
    private int roomMaxX;
    private int roomMinY;
    private int roomMaxY;

    // Start is called before the first frame update
    void Start()
    {
        ObjectFromTilemap(tilemap);
    }

    private void ObjectFromTilemap(Tilemap tilemap)
    {
        List<Tile> tiles = new List<Tile>();
        List<Vector3Int> positions = new List<Vector3Int>();

        // Add tiles to list
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int currentPosition = new Vector3Int(x, y, 0);
                Tile tile = tilemap.GetTile<Tile>(currentPosition);

                if (tile != null)
                {
                    tiles.Add(tile);
                    positions.Add(currentPosition);

                    CalculateBorder(x, y);
                }
            }
        }

        int width = roomMaxX - roomMinX;
        int height = roomMaxY - roomMinY;

        positions = ResetCoordinates(positions, width, height);

        // Create empty gameobject, add "Tile" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>(); ;

        room.tiles = tiles.ToArray();
        room.positions = positions.ToArray();
        room.tilemap = tilemap;
        room.SetPosition(Vector3Int.zero);
        room.SetWidth(width); ;
        room.SetHeight(height);

        tilemap.ClearAllTiles();

        room.CreateRoom();
    }

    private List<Vector3Int> ResetCoordinates(List<Vector3Int> positions, int width, int height)
    {
        Vector3Int[] positionsArray = positions.ToArray(); 

        int xOffset = 0 - positionsArray[0].x;
        int yOffset = 0 - positionsArray[0].y;

        int widthOffset = width / 2;
        int heightOffset = height / 2;

        for (int i = 0; i < positions.Count; i++)
        {
            positionsArray[i].x += xOffset;
            positionsArray[i].y += yOffset;
        }

        return new List<Vector3Int>(positionsArray);
    }

    private void CalculateBorder(int x, int y)
    {
        if (x < roomMinX)
            roomMinX = x;
        if (x > roomMaxX)
            roomMaxX = x;
        if (y < roomMinY)
            roomMinY = y;
        if (y > roomMaxY)
            roomMaxY = y;
    }
}
