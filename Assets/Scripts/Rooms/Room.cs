using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum direction
{
    Up,
    Down,
    Right,
    Left
}

public struct Door
{
    public Door(Vector2Int position, direction dir)
    {
        Position = position;
        Direction = dir;
    }

    public Vector2Int Position { get; set; }

    public direction Direction { get; }
}

public class Room : MonoBehaviour
{
    public Tile[] tiles;

    public Vector2Int[] positions;
    public Vector2Int offsetPosition;

    public Tilemap tilemap;

    public int width = 0;
    public int height = 0;

    private List<direction> ignoredDoors;

    // Start is called before the first frame update
    void Awake()
    {
        ignoredDoors = new List<direction>();
        tilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    }

    public void DrawRoom()
    {
        if(tilemap == null)
            return;

        if (offsetPosition == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap.SetTile((Vector3Int)(positions[i] + offsetPosition), tiles[i]);
        }
    }

    public List<Door> GetDoorPositions()
    {
        List<Door> tempPos = new List<Door>();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].sprite.name == "DoorUp" && !ignoredDoors.Contains(direction.Up))
                tempPos.Add(new Door(positions[i], direction.Up));

            else if (tiles[i].sprite.name == "DoorDown" && !ignoredDoors.Contains(direction.Down))
                tempPos.Add(new Door(positions[i], direction.Down));

            else if (tiles[i].sprite.name == "DoorLeft" && !ignoredDoors.Contains(direction.Left))
                tempPos.Add(new Door(positions[i], direction.Left));

            else if (tiles[i].sprite.name == "DoorRight" && !ignoredDoors.Contains(direction.Right))
                tempPos.Add(new Door(positions[i], direction.Right));
        }
        return tempPos;
    }

    internal void RemoveDoor(direction dir)
    {
        ignoredDoors.Add(dir);
    }
    internal void SetWidth(int v)
    {
        width = v;
    }

    internal void SetHeight(int v)
    {
        height = v;
    }

    public void SetPosition(Vector2Int pos)
    {
        offsetPosition = pos;
    }

    public  void SetTileMap(Tilemap _tilemap) 
    {
        tilemap = _tilemap;
    }
}
