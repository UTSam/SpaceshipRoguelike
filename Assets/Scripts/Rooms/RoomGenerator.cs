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
    List<Vector2Int> positions = new List<Vector2Int>();

    public GameObject GetGameObject()
    {
        return GetRoom().gameObject;
    }

    private Room GetRoom()
    {
        GenerateFromTilemap();

        RectSimpl rect = GetDimensions();

        SetCoordinatesToTopLeft();

        // Create empty gameobject, add "Tile" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>(); ;

        room.tiles = tiles.ToArray();
        room.tilePositions = positions.ToArray();
        room.tilemap = tilemap;
        room.position = Vector2Int.zero;
        room.width = rect.width;
        room.height = rect.height;

        return room;
    }

    // Go through the tilemap and saves every tile / position in two lists.
    private void GenerateFromTilemap()
    {
        // Add tiles to list
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector2Int currentPosition = new Vector2Int(x, y);
                Tile tile = tilemap.GetTile<Tile>((Vector3Int)currentPosition);

                if (tile != null)
                {
                    tiles.Add(tile);
                    positions.Add(currentPosition);
                }
            }
        }
    }

    private RectSimpl GetDimensions()
    {
        minPosition.x = positions.Min(vector3 => vector3.x);
        minPosition.y = positions.Min(vector3 => vector3.y);

        maxPosition.x = positions.Max(vector3 => vector3.x);
        maxPosition.y = positions.Max(vector3 => vector3.y);

        int width = maxPosition.x - minPosition.x;
        int height = maxPosition.y - minPosition.y;

        return new RectSimpl(width, height);
    }

    // Since the user can draw anywhere in the world space we need to reset the room
    // to Vector2.zero. This way when we spawn a room it will have an consistant
    // location. The left bottom of the room will be set to 0,0.
    private void SetCoordinatesToTopLeft()
    {
        Vector2Int[] newPositions = positions.ToArray();

        int xOffset = 0-minPosition.x;
        int yOffset = 0-minPosition.y;

        for (int i = 0; i < positions.Count; i++)
        {
            newPositions[i].x += xOffset;
            newPositions[i].y += yOffset;
        }

        positions = new List<Vector2Int>(newPositions);
    }
}
