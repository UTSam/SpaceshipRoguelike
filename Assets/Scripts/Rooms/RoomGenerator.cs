using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    public BoundsInt area;
    public Tilemap tilemap;

    public Vector2Int minPosition;
    public Vector2Int maxPosition;

    public Tile center;

    public GameObject GetGameObject()
    {
        return GetRoomFromTilemap().gameObject;
    }

    public void DrawTilemap()
    {
        Room room = GetRoomFromTilemap();
    }

    private Room GetRoomFromTilemap()
    {
        List<Tile> tiles = new List<Tile>();
        List<Vector2Int> positions = new List<Vector2Int>();

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

        minPosition.x = positions.Min(vector3 => vector3.x);
        minPosition.y = positions.Min(vector3 => vector3.y);

        maxPosition.x = positions.Max(vector3 => vector3.x);
        maxPosition.y = positions.Max(vector3 => vector3.y);

        int width = maxPosition.x - minPosition.x;
        int height = maxPosition.y - minPosition.y;


        positions = SetCoordinatesToTopLeft(positions);

        // Create empty gameobject, add "Tile" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>(); ;

        room.tiles = tiles.ToArray();
        room.positions = positions.ToArray();
        room.tilemap = tilemap;
        room.SetPosition(Vector2Int.zero);
        room.SetWidth(width);
        room.SetHeight(height);

        return room;
    }

    // Since the user can draw anywhere in the world space we need to reset the room
    // to Vector2.zero. This way when we spawn a room it will have an consistant
    // location
    private List<Vector2Int> SetCoordinatesToTopLeft(List<Vector2Int> positions)
    {
        Vector2Int[] positionsArray = positions.ToArray();

        int xOffset = 0-minPosition.x;
        int yOffset = 0-minPosition.y;

        for (int i = 0; i < positions.Count; i++)
        {
            positionsArray[i].x += xOffset;
            positionsArray[i].y += yOffset;
        }

        return new List<Vector2Int>(positionsArray);
    }
}
