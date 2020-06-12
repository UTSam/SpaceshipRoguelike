/*
    Authors:
      Robbert Ritsema
      Jacco Douma
*/

using Assets.Scripts.Dungeon;
using System;
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

    public List<Assets.Scripts.Dungeon.TileData> tileDataArray = new List<Assets.Scripts.Dungeon.TileData>();

    public Vector3Int globalPosition;

    [SerializeField]
    public List<Door> doors = new List<Door>();

    public Room previousRoom;

    public bool isCleared = false;
    private List<BoxCollider2D> colliderList = new List<BoxCollider2D>();

    [SerializeField]
    private int numberOfEnemiesMin;
    [SerializeField]
    private int numberOfEnemiesMax;

    public bool playerEntered = false;
    private bool openedDoors = false;
    public bool spawnPatrick = false;

    private void Update()
    {
        OpenRoomIfEnemiesAreDead();
    }

    private void OpenRoomIfEnemiesAreDead()
    {
        if (!playerEntered) return;
        if (openedDoors) return;

        BasicMovingEnemy[] gameObjects = GetComponentsInChildren<BasicMovingEnemy>(true) as BasicMovingEnemy[];

        if (gameObjects.Length == 0)
        {
            this.OpenDoors();
            openedDoors = true;
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        if (spawnPatrick)
        {
            Instantiate(GVC.Instance.Patrick, this.globalPosition, Quaternion.identity, this.transform);
        }

        if (UnityEngine.Random.value < 0.2f)
        {
            GameObject pack = Instantiate(GVC.Instance.chest) as GameObject;
            pack.transform.position = transform.position;
        }
    }

    public void DrawRoom(Tilemap drawOnThis = null)
    {
        foreach(Assets.Scripts.Dungeon.TileData pls in tileDataArray)
        {
            pls.SpawnTiles(globalPosition, drawOnThis);
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
                door.Unlock(this.globalPosition);
        }
    }

    public void CloseDoors()
    {
        foreach (Door door in doors)
        {
            if (door.connected)
                door.Lock(this.globalPosition);
        }
    }

    public void AddDoorTriggers()
    {
        if (isCleared) return;

        foreach (Door door in doors)
        {
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

            GVC.Instance.Minimap.Generate();
        }
    }

    // Spawn the enemies over the whole room.
    public void SpawnEnemies()
    {
        // In an extremely rare case this could freeze the game, so just making sure that it doesn't
        System.Random rnd = new System.Random();

        // Get the correct tiledate
        Assets.Scripts.Dungeon.TileData tileData = null;
        foreach (Assets.Scripts.Dungeon.TileData _tileData in tileDataArray)
        {
            if(_tileData.tilemapName == "Floors")
            {
                tileData = _tileData;
            }
        }

        List<Vector3Int> spawnablePositions = new List<Vector3Int>();
        //Remove position we dont want the enemies to spawn in
        for (int i = 0; i < tileData.tileLocalPositions.Count; i++)
        {
            Vector3Int tilePos = tileData.tileLocalPositions[i];
            if (PositionIsNearDoor(tilePos))
                continue;

            spawnablePositions.Add(tilePos);
        }

        int amountOfEnemies = rnd.Next(numberOfEnemiesMin, numberOfEnemiesMax);
        for (int i = 0; i < amountOfEnemies; i++)
        {
            int spawnablePositionsIndex = rnd.Next(spawnablePositions.Count);
            Vector3Int spawnPosition = spawnablePositions[spawnablePositionsIndex] + globalPosition;

            int enemieIndex = rnd.Next(GVC.Instance.enemies.Count);
            GameObject enemy = GVC.Instance.enemies[enemieIndex];

            Instantiate(enemy, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    // Little yikes code, but kinda works.
    // Get a random position in the room. Also make sure it isn't to close to a door
    private bool PositionIsNearDoor(Vector3Int spawnPosition)
    {
        int maxDistance = 8;

        foreach (Door door in doors)
        {
            float distance = Vector3.Distance(spawnPosition, door.position + this.globalPosition);
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

        Gizmos.DrawSphere(globalPosition + new Vector3(rb.xMin, rb.yMin), .2f);
        Gizmos.DrawSphere(globalPosition + new Vector3(rb.xMin, rb.yMax), .2f);
        Gizmos.DrawSphere(globalPosition + new Vector3(rb.xMax, rb.yMin), .2f);
        Gizmos.DrawSphere(globalPosition + new Vector3(rb.xMax, rb.yMax), .2f);

        Gizmos.DrawLine(
            globalPosition + new Vector3(rb.xMax, rb.yMin),
            globalPosition + new Vector3(rb.xMin, rb.yMin));
        Gizmos.DrawLine(
            globalPosition + new Vector3(rb.xMax, rb.yMax),
            globalPosition + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            globalPosition + new Vector3(rb.xMin, rb.yMin),
            globalPosition + new Vector3(rb.xMin, rb.yMax));
        Gizmos.DrawLine(
            globalPosition + new Vector3(rb.xMax, rb.yMax),
            globalPosition + new Vector3(rb.xMax, rb.yMin));

        if (previousRoom)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(globalPosition, previousRoom.globalPosition);
        }
    }
}