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
        room = ResetRoomToCenter(room);
        room.SetDoors();

        return room;
    }

    // Set the position of the room to the center
    private Room ResetRoomToCenter(Room room)
    {
        Vector3Int[] tilePositions = room.tilePositions;
        Room.RoomBorders rb = room.roomBorders;

        int dx = -(rb.xMax + rb.xMin) / 2;
        int dy = -(rb.yMax + rb.yMin) / 2;

        // Change tile positions
        for (int i = 0; i < tilePositions.Length; i++)
        {
            tilePositions[i].x += dx; 
            tilePositions[i].y += dy; 
        }

        // Change room borders
        rb.xMin += dx;
        rb.xMax += dx;
        rb.yMin += dy;
        rb.yMax += dy; 

        room.tilePositions = tilePositions;
        room.roomBorders = rb; 

        return room;
    }
}
