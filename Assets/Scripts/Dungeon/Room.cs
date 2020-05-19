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
    public GameObject[] possibleEnemies;

    public void DrawRoom()
    {
        if (DungeonManager.tilemap_walls == null)
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
            DungeonManager.tilemap_walls.SetTile((tilePositions[i] + position), tiles[i]);
        }
    }

    public void SetDoorConnected(Door findDoor)
    {
        foreach (Door door in doors)
        {
            if(door == findDoor)
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
            if(door.direction == direction)
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
            // TODO FIX
            //if (!door.connected) return;

            // Add trigger in front of door 
            BoxCollider2D collider = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            collider.isTrigger = true;

            Vector2Int size = new Vector2Int(1,1);
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
        if (collision.gameObject.tag != "Player") return;

        // Close room & Spawn enemies
        if (!isCleared)
        {
            this.CloseDoors();
            this.SpawnEnemies();
            isCleared = true;

            // TEMP
            StartCoroutine(OpenAfterSeconds(5));

            // TODO: Maybe remove colliders (There's no use for them anyways)
            foreach (BoxCollider2D bc in colliderList)
            {
                Destroy(bc);
            }
            colliderList.Clear();
        }
    }

    public void SpawnEnemies()
    {
        foreach (GameObject enemie in possibleEnemies)
        {
            Instantiate(enemie, this.transform);
            ShootingComponent shooting = enemie.GetComponent<ShootingComponent>();
            BasicMovingEnemy basicEnemie = enemie.GetComponent<BasicMovingEnemy>();
            if (basicEnemie && shooting)
            {
                shooting.Target = DungeonManager.GetPlayer().transform;
                basicEnemie.target = DungeonManager.GetPlayer();
            }
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