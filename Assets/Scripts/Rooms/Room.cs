using Assets.Scripts.Rooms;
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

    public Vector3Int position;

    public List<Door> doors = new List<Door>();

    public Room previousRoom;

    // Start is called before the first frame update
    void Awake()
    {
        tilemap_walls = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();

        SetDoors();
    }

    public void DrawRoom()
    {
        if(tilemap_walls == null)
            return;

        if (position == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            tilemap_walls.SetTile((tilePositions[i] + position), tiles[i]);
        }
    }

    internal void AddToPosition(Vector3Int addToPosition)
    {
        position += addToPosition;
    }

    public void RemoveDoor(Door door)
    {
        doors.Remove(door);
    }

    public void SetDoors()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            Door newDoor = new Door(new Vector3Int(tilePositions[i].x, tilePositions[i].y, 0));

            switch (tiles[i].sprite.name)
            {
                case "DoorUp":
                    newDoor.Direction = Direction.Up;
                    break;
                case "DoorDown":
                    newDoor.Direction = Direction.Down;
                    break;
                case "DoorLeft":
                    newDoor.Direction = Direction.Left;
                    break;
                case "DoorRight":
                    newDoor.Direction = Direction.Right;
                    break;
                default:
                    continue;
            }

            doors.Add(newDoor);
        }
    }

    public Door GetRandomDoor(System.Random rand)
    {
        return doors[rand.Next(doors.Count)];
    }

    public Door GetDoorByDirection(Direction direction)
    {
        foreach (Door door in doors)
        {
            if(door.Direction == direction)
            {
                return door;
            }
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RoomBorders rb = roomBorders;

        Gizmos.DrawSphere(position + new Vector3(rb.xMin, rb.yMin), .2f);
        Gizmos.DrawSphere(position + new Vector3(rb.xMin, rb.yMax), .2f);
        Gizmos.DrawSphere(position + new Vector3(rb.xMax, rb.yMin), .2f);
        Gizmos.DrawSphere(position + new Vector3(rb.xMax, rb.yMax), .2f);

        Gizmos.DrawLine(
            position + new Vector3(rb.xMax, rb.yMin),
            position + new Vector3(rb.xMin, rb.yMin));
        Gizmos.DrawLine(
            position + new Vector3(rb.xMax, rb.yMax),
            position + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            position + new Vector3(rb.xMin, rb.yMin),
            position + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            position + new Vector3(rb.xMax, rb.yMax),
            position + new Vector3(rb.xMax, rb.yMin));

        foreach(Door door in doors)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(door.Position + position, 1f);
        }


        if (previousRoom)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, previousRoom.position);
        }
    }
}