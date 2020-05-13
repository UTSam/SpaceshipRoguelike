using Assets.Scripts.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
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

    public Tile[] tiles;
    public Vector3Int[] tilePositions;

    public Vector3Int position;

    [SerializeField]
    public List<Door> doors = new List<Door>();
    [SerializeField]

    public Room previousRoom;

    public void DrawRoom()
    {
        //SetDoors();
        if(DungeonManager.tilemap_walls == null)
            return;

        if (position == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            DungeonManager.tilemap_walls.SetTile((tilePositions[i] + position), tiles[i]);
        }
    }

    internal void AddToPosition(Vector3Int addToPosition)
    {
        position += addToPosition;
    }

    public void DoorIsConnected(Door findDoor)
    {
        foreach (Door door in doors)
        {
            if(door == findDoor)
            {
                door.connected = true;
            }
        }
    }

    public Door GetRandomDoor(System.Random rand)
    {
        foreach (Door door in doors.OrderBy(x => rand.Next()))
        {
            if (door.connected == false)
            {
                return door;
            }
        }

        return null;
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

    public void OpenDoors()
    {
        foreach (Door door in doors)
        {
            if (door.connected) 
                door.Unlock(this.position);
        }
    }

    public void CloseDoors()
    {
        foreach (Door door in doors)
        {
            if (door.connected) 
                door.Lock(this.position);
        }
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

        if (previousRoom)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, previousRoom.position);
        }
    }
}