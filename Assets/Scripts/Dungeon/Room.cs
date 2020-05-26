﻿using Assets.Scripts.Dungeon;
using Assets.Scripts.Rooms;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [System.Serializable]
    public struct RoomBorders
    {
        public int xMin, xMax, yMin, yMax;
    }

    public RoomBorders roomBorders;

    public List<PleaveGiveMeGoodName> tileDataArray = new List<PleaveGiveMeGoodName>();

    public Vector3Int position;

    [SerializeField]
    public List<Door> doors = new List<Door>();

    public Room previousRoom;

    public bool isCleared = false;
    private List<BoxCollider2D> colliderList = new List<BoxCollider2D>();

    [SerializeField]
    private List<GameObject> enemiesToSpawn;

    private bool playerEntered = false;
    private bool openedDoors = false;

    private void Update()
    {
        OpenRoomIfEnemiesAreDead();
    }

    private void OpenRoomIfEnemiesAreDead()
    {
        if (!playerEntered) return;
        if (openedDoors) return;

        // TODO FIX: Performance heavy probably
        BasicMovingEnemy[] gameObjects = GetComponentsInChildren<BasicMovingEnemy>(true) as BasicMovingEnemy[];

        if (gameObjects.Length == 0)
        {
            this.OpenDoors();
            openedDoors = true;
        }
    }

    public void DrawRoom(Tilemap drawOnThis = null)
    {
        foreach(PleaveGiveMeGoodName pls in tileDataArray)
        {
            pls.SpawnTiles(position, drawOnThis);
        }
    }

    public void SetDoorConnected(Door findDoor)
    {
        foreach (Door door in doors)
        {
            if (door == findDoor)
                door.connected = true;
        }
    }

    public Door GetRandomDoor(System.Random rand)
    {
        foreach (Door door in doors.OrderBy(x => rand.Next()))
        {
            if (door.connected == false)
                return door;
        }

        return null;
    }

    public Door GetDoorByDirection(Direction direction)
    {
        foreach (Door door in doors)
        {
            if (door.direction == direction)
                return door;
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

    public void AddDoorTriggers()
    {
        if (isCleared) return;

        foreach (Door door in doors)
        {
            // TODO FIX
            //if (!door.connected) return;

            // Add trigger in front of door 
            BoxCollider2D collider = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            collider.isTrigger = true;

            Vector2Int size = new Vector2Int(1, 1);
            Vector2 offset = new Vector2(door.position.x + 0.5f, door.position.y + 0.5f);

            switch (door.direction)
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

            collider.size = size;
            collider.offset = offset;
            colliderList.Add(collider);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player collision
        if (!collision.gameObject.CompareTag("Player")) return;

        playerEntered = true;

        // Close room & Spawn enemies
        if (!isCleared)
        {
            this.CloseDoors();
            //this.SpawnEnemies();
            isCleared = true;

            foreach (BoxCollider2D bc in colliderList)
            {
                Destroy(bc);
            }
            colliderList.Clear();
        }
    }

    // Spawn the enemies over the whole room.
    public void SpawnEnemies(System.Random rnd)
    {
        // In an extremely rare case this could freeze the game, so just making sure that it doesn't
        int maxNumberOfTries = 100;

        foreach (GameObject enemy in enemiesToSpawn)
        {
            int numberOfTries = 0;
            bool didntFoundFloorTile = true;

            while (didntFoundFloorTile)
            {
                // Get an random position which tries to be inside the room
                Vector3Int spawnPosition = new Vector3Int(
                    (int)transform.position.x + rnd.Next(roomBorders.xMin, roomBorders.xMax),
                    (int)transform.position.y + rnd.Next(roomBorders.yMin, roomBorders.yMax), 
                    0);
                if (PositionIsNearDoor(spawnPosition)) continue;

                Tile tile = GVC.Instance.tilemap.floor.GetTile<Tile>(spawnPosition);
                if (tile != null && tile.name.StartsWith("Floor"))
                {
                    Instantiate(enemy, spawnPosition, Quaternion.identity, this.transform);
                    didntFoundFloorTile = false;
                }
                else
                {
                    if (numberOfTries++ >= maxNumberOfTries)
                        didntFoundFloorTile = false;
                }

            }
        }
    }

    // Little yikes code, but kinda works.
    // Get a random position in the room. Also make sure it isn't to close to a door
    private bool PositionIsNearDoor(Vector3Int spawnPosition)
    {
        int maxDistance = 8;

        foreach (Door door in doors)
        {
            float distance = Vector3.Distance(spawnPosition, door.position + this.position);
            if (distance <= maxDistance)
            {
                return true;
            }
        }

        return false;
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