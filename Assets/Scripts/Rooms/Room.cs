using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public struct Door
{
    public Vector2Int Position { get; set; }

    public Direction Direction { get; }

    public Door(Vector2Int position, Direction direction)
    {
        Position = position;
        Direction = direction;
    }
}

public class Room : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile[] tiles;
    public Vector2Int[] tilePositions;

    public Vector2Int position;

    public int width;
    public int height;

    private List<Direction> ignoredDoors;

    // Start is called before the first frame update
    void Awake()
    {
        ignoredDoors = new List<Direction>();
        tilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    }

    public void DrawRoom()
    {
        if(tilemap == null)
            return;

        if (position == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap.SetTile((Vector3Int)(tilePositions[i] + position), tiles[i]);
        }
    }

    public List<Door> GetDoorPositions()
    {
        List<Door> tempPos = new List<Door>();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].sprite.name == "DoorUp" && !ignoredDoors.Contains(Direction.Up))
                tempPos.Add(new Door(tilePositions[i], Direction.Up));

            else if (tiles[i].sprite.name == "DoorDown" && !ignoredDoors.Contains(Direction.Down))
                tempPos.Add(new Door(tilePositions[i], Direction.Down));

            else if (tiles[i].sprite.name == "DoorLeft" && !ignoredDoors.Contains(Direction.Left))
                tempPos.Add(new Door(tilePositions[i], Direction.Left));

            else if (tiles[i].sprite.name == "DoorRight" && !ignoredDoors.Contains(Direction.Right))
                tempPos.Add(new Door(tilePositions[i], Direction.Right));
        }
        return tempPos;
    }

    internal void RemoveDoor(Direction dir)
    {
        ignoredDoors.Add(dir);
    }

    internal void AddToPosition(Vector2Int addToPosition)
    {
        position += addToPosition;
    }
}
