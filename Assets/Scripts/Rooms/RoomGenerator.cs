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

    private Vector2Int minPosition;
    private Vector2Int maxPosition;

    private List<Tile> tiles = new List<Tile>();
    List<Vector3Int> positions = new List<Vector3Int>();

    public GameObject GetGameObject()
    {
        return GetRoom().gameObject;
    }

    public Room GetRoom()
    {
        List<Tile> tiles = new List<Tile>();
        List<Vector3Int> pos = new List<Vector3Int>();

        Room.RoomBorders borders;

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
                    if (x < borders.xMin) { borders.xMin = x; }
                    if (y < borders.yMin) { borders.yMin = y; }
                    if (x > borders.xMax) { borders.xMax = x; }
                    if (y > borders.yMax) { borders.yMax = y; }

                    Debug.Log(tile.sprite.name);
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

        return room;
    }

    //private Room GetRoom()
    //{
    //    GenerateFromTilemap();

    //    RectSimpl rect = GetDimensions();

    //    SetCoordinatesToTopLeft();

    //    // Create empty gameobject, add "Tile" script and populate values
    //    GameObject newObj = new GameObject("Room");
    //    Room room = newObj.AddComponent<Room>(); ;

    //    room.tiles = tiles.ToArray();
    //    room.tilePositions = positions.ToArray();
    //    room.tilemap_walls = tilemap;
    //    room.pos = Vector3Int.zero;
    //    room.width = rect.width;
    //    room.height = rect.height;

    //    // Fill roomBorders
    //    room.roomBorders.xMin = positions.Min(vector3 => vector3.x);
    //    room.roomBorders.xMax = positions.Max(vector3 => vector3.x);
    //    room.roomBorders.yMin = positions.Min(vector3 => vector3.y);
    //    room.roomBorders.yMax = positions.Max(vector3 => vector3.y);

    //    return room;
    //}

    //// Go through the tilemap and saves every tile / position in two lists.
    //private void GenerateFromTilemap()
    //{
    //    // Add tiles to list
    //    for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
    //    {
    //        for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
    //        {
    //            Vector2Int currentPosition = new Vector2Int(x, y);
    //            Tile tile = tilemap.GetTile<Tile>((Vector3Int)currentPosition);

    //            if (tile != null)
    //            {
    //                tiles.Add(tile);
    //                positions.Add((Vector3Int) currentPosition);
    //            }
    //        }
    //    }
    //}

    //private RectSimpl GetDimensions()
    //{
    //    minPosition.x = positions.Min(vector3 => vector3.x);
    //    minPosition.y = positions.Min(vector3 => vector3.y);

    //    maxPosition.x = positions.Max(vector3 => vector3.x);
    //    maxPosition.y = positions.Max(vector3 => vector3.y);

    //    int width = maxPosition.x - minPosition.x;
    //    int height = maxPosition.y - minPosition.y;

    //    return new RectSimpl(width, height);
    //}

    //// Since the user can draw anywhere in the world space we need to reset the room
    //// to Vector2.zero. This way when we spawn a room it will have an consistant
    //// location. The left bottom of the room will be set to 0,0.
    //private void SetCoordinatesToTopLeft()
    //{
    //    Vector3Int[] newPositions = positions.ToArray();

    //    int xOffset = 0-minPosition.x;
    //    int yOffset = 0-minPosition.y;

    //    for (int i = 0; i < positions.Count; i++)
    //    {
    //        newPositions[i].x += xOffset;
    //        newPositions[i].y += yOffset;
    //    }

    //    positions = new List<Vector3Int>(newPositions);
    //}
}
