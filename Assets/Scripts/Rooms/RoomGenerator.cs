using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public struct RectSimpl
{
    public int width;
    public int height;

    public RectSimpl(int _width, int _height)
    {
        width = _width;
        height = _height;
    }
}

public class RoomGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    Room.RoomBorders borders;

    public GameObject GetGameObject()
    {
        return GetRoom().gameObject;
    }

    public Room GetRoom()
    {
        List<Tile> tiles = new List<Tile>();
        List<Vector3Int> pos = new List<Vector3Int>();

        // Reset borders
        borders.xMin = int.MaxValue;
        borders.yMin = int.MaxValue;
        borders.xMax = int.MinValue;
        borders.yMax = int.MinValue;

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

                    // Update borders
                    if (x < borders.xMin) borders.xMin = x;
                    if (y < borders.yMin) borders.yMin = y;
                    if (x > borders.xMax) borders.xMax = x;
                    if (y > borders.yMax) borders.yMax = y;
                }
            }
        }

        // Because of tiles, we need to up the max X and Y 
        borders.xMax++;
        borders.yMax++;

        // Create empty gameobject, add "Room" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>();
        room.tiles = tiles.ToArray();
        room.tilePositions = pos.ToArray();
        room.tilemap_walls = tilemap;
        room.roomBorders = borders;

        room.SetDoors();

        return room;
    }

    // Since the user can draw anywhere in the world space we need to reset the room
    // to Vector2.zero. This way when we spawn a room it will have an consistent
    // location. The left bottom of the room will be set to 0,0.
    private List<Vector3Int> SetCoordinatesToBottomLeft(List<Vector3Int> positions)
    {
        Vector3Int[] newPositions = positions.ToArray();

        int xOffset = 0 - borders.xMin;
        int yOffset = 0 - borders.yMin;

        for (int i = 0; i < positions.Count; i++)
        {
            newPositions[i].x += xOffset;
            newPositions[i].y += yOffset;
        }

        return new List<Vector3Int>(newPositions);
    }
}
