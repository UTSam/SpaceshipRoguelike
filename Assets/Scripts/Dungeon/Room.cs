using Assets.Scripts.Rooms;
using System;
using System.Collections;
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

    public Tile[] tiles;
    public Vector3Int[] tilePositions;

    public Vector3Int position;

    [SerializeField]
    public List<Door> doors = new List<Door>();

    public Room previousRoom;

    public bool isCleared = false;
    private List<BoxCollider2D> colliderList = new List<BoxCollider2D>();

    [SerializeField]
    private List<GameObject> enemiesToSpawn;

    internal bool lastRoom = false;
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

    public void DrawRoom()
    {
        if (GVC.Instance.tilemap.walls == null)
        {
            Debug.LogError("DungeonManager.tilemap_walls == null in drawroom");
            return;
        }

        if (position == null)
        {
            Debug.LogError("position == null in drawroom");
            return;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == null) continue;

            // TODO FIX this bullshit
            if(tiles[i].name.StartsWith("floor") || tiles[i].name.StartsWith("Floor"))
            {
                GVC.Instance.tilemap.floor.SetTile((tilePositions[i] + position), tiles[i]);
            }
            else
            {
                GVC.Instance.tilemap.walls.SetTile((tilePositions[i] + position), tiles[i]);
            }
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
            this.SpawnEnemies();
            isCleared = true;

            foreach (BoxCollider2D bc in colliderList)
            {
                Destroy(bc);
            }
            colliderList.Clear();
        }
    }

    public void SpawnEnemies()
    {
        System.Random rnd = new System.Random();
        int offsetRandom = 5;

        foreach (GameObject enemy in enemiesToSpawn)
        {

            Vector3 postition = new Vector3(transform.position.x + rnd.Next(-offsetRandom, offsetRandom), transform.position.y + rnd.Next(-offsetRandom, offsetRandom), 0);

            // Check if position is in the room
            Instantiate(enemy, postition, Quaternion.identity, this.transform);
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