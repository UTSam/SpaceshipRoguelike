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

    public Room previousRoom;

    public bool isCleared = false; 
    private List<BoxCollider2D> bclist = new List<BoxCollider2D>();

    public void DrawRoom()
    {
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

    public void AddTriggers()
    {
        if (isCleared) return;

        foreach (Door door in doors)
        {
            if (door.connected)
            {
                // Add trigger in front of door 
                BoxCollider2D bc = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                bc.isTrigger = true;

                Vector2Int size = new Vector2Int(1,1);
                Vector2 offset = new Vector2(door.Position.x + 0.5f, door.Position.y + 0.5f);

                switch (door.Direction)
                {
                    case Direction.Up:
                        size.x = 3;
                        offset.y -= 1.5f;
                        break;
                    case Direction.Down:
                        size.x = 3; 
                        offset.y += 1.5f;
                        break;
                    case Direction.Left:
                        size.y = 3;
                        offset.x += 1.5f;
                        break;
                    case Direction.Right:
                        size.y = 3; 
                        offset.x -= 1.5f;
                        break;
                }

                bc.size = size;
                bc.offset = offset;
                bclist.Add(bc);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player collision
        if (collision.gameObject.tag != "Player") return;

        // Close room & Spawn enemies
        if (!isCleared)
        {
            this.CloseDoors();
            isCleared = true;

            // TEMP
            StartCoroutine(OpenAfterSeconds(5));

            // TODO: Maybe remove colliders (There's no use for them anyways)
            foreach (BoxCollider2D bc in bclist)
            {
                Destroy(bc);
            }
            bclist.Clear();
        }
    }

    IEnumerator OpenAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.OpenDoors();
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