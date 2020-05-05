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
    [System.Serializable]
    public struct RoomBorders
    {
        public int xMin, xMax, yMin, yMax;
    }

    public RoomBorders roomBorders;

    public Tilemap tilemap_walls;

    public Tile[] tiles;
    public Vector3Int[] tilePositions;

    public Vector3Int pos;

    public int width;
    public int height;

    private List<Direction> ignoredDoors;


    // List with placed tiles, to be removed when moving the thing
    private List<Vector3Int> placedTiles = new List<Vector3Int>();

    // Start is called before the first frame update
    void Awake()
    {
        ignoredDoors = new List<Direction>();
        tilemap_walls = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
    }

    public void DrawRoom()
    {
        if(tilemap_walls == null)
            return;

        if (pos == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap_walls.SetTile((tilePositions[i] + pos), tiles[i]);
        }
    }

    public List<Door> GetDoorPositions()
    {
        List<Door> tempPos = new List<Door>();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].sprite.name == "DoorUp" && !ignoredDoors.Contains(Direction.Up))
                tempPos.Add(new Door(new Vector2Int(tilePositions[i].x, tilePositions[i].y), Direction.Up));

            else if (tiles[i].sprite.name == "DoorDown" && !ignoredDoors.Contains(Direction.Down))
                tempPos.Add(new Door(new Vector2Int(tilePositions[i].x, tilePositions[i].y), Direction.Down));

            else if (tiles[i].sprite.name == "DoorLeft" && !ignoredDoors.Contains(Direction.Left))
                tempPos.Add(new Door(new Vector2Int(tilePositions[i].x, tilePositions[i].y), Direction.Left));

            else if (tiles[i].sprite.name == "DoorRight" && !ignoredDoors.Contains(Direction.Right))
                tempPos.Add(new Door(new Vector2Int(tilePositions[i].x, tilePositions[i].y), Direction.Right));
        }
        return tempPos;
    }

    internal void RemoveDoor(Direction dir)
    {
        ignoredDoors.Add(dir);
    }

    internal void AddToPosition(Vector2Int addToPosition)
    {
        pos += (Vector3Int) addToPosition;
    }

    private void OnDrawGizmos()
    {
        RoomBorders rb = roomBorders;

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(pos + new Vector3(rb.xMin, rb.yMin), .2f);
        Gizmos.DrawSphere(pos + new Vector3(rb.xMin, rb.yMax), .2f);
        Gizmos.DrawSphere(pos + new Vector3(rb.xMax, rb.yMin), .2f);
        Gizmos.DrawSphere(pos + new Vector3(rb.xMax, rb.yMax), .2f);

        Gizmos.DrawLine(
            pos + new Vector3(rb.xMax, rb.yMin),
            pos + new Vector3(rb.xMin, rb.yMin));
        Gizmos.DrawLine(
            pos + new Vector3(rb.xMax, rb.yMax),
            pos + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            pos + new Vector3(rb.xMin, rb.yMin),
            pos + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            pos + new Vector3(rb.xMax, rb.yMax),
            pos + new Vector3(rb.xMax, rb.yMin));
    }
}
