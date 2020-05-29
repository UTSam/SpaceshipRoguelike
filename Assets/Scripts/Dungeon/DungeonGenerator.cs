using Assets.Scripts.Dungeon;
using Assets.Scripts.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] public int seed = 0;

    private List<Room> availableRooms = new List<Room>();
    private List<Room> placedRooms = new List<Room>();

    [SerializeField] private Dictionary<Direction, List<Room>> roomsByDirection = new Dictionary<Direction, List<Room>>();
    [SerializeField] private int count = 0;
    [SerializeField] private int roomCount = 50;
    [SerializeField] private int additionalDistance = 4;
    [SerializeField] private int maxOffset = 20;
    [SerializeField] private GameObject patrick;

    private float startTime;
    private Transform parentFolder;


    public void Start()
    {
        parentFolder = this.transform.Find("Rooms");
        LoadAllRoomsInResources();

        startTime = Time.time;
        StartCoroutine(GenerateDungeon());
    }

    IEnumerator GenerateDungeon()
    {
        System.Random rand = new System.Random(seed);

        // Place starting room
        Room spawnRoom = Instantiate(GetAndRemoveStartingRoom(), parentFolder);
        placedRooms.Add(spawnRoom);
        spawnRoom.DrawRoom();
        spawnRoom.isCleared = true;

        bool dontExit = true; 

        // Keep placing rooms until roomCount
        while (count < roomCount && dontExit)
        {
            // Exit if stuck (:
            if ((Time.time - startTime) > 10)
            {
                dontExit = false;
            }

            // Pick random room from placed rooms
            Room initialRoom = placedRooms[rand.Next(placedRooms.Count)];
            if (initialRoom.doors.Count == 0)
                continue;

            // Get a room based on the direcion of the door
            Door door = initialRoom.GetRandomDoor(rand);
            if (door == null)
                continue;

            Room roomToConnect = GetRoomByDirection(door.GetOppositeDirection(), rand);
            if(roomToConnect == null)
            {
                dontExit = false;
                continue;
            }

            int randomOffset = rand.Next(maxOffset * 2) - maxOffset;
            Vector3Int newRoomPosition = Vector3Int.zero;

            // Set the room offset position based on the direction of the door
            if (door.direction == Direction.Down)
            {
                newRoomPosition -= new Vector3Int(0, -initialRoom.roomBorders.yMin, 0);
                newRoomPosition -= new Vector3Int(0, roomToConnect.roomBorders.yMax, 0);
                newRoomPosition -= new Vector3Int(randomOffset, additionalDistance, 0);
            }
            else if (door.direction == Direction.Left)
            {
                newRoomPosition -= new Vector3Int(-initialRoom.roomBorders.xMin, 0, 0);
                newRoomPosition -= new Vector3Int(roomToConnect.roomBorders.xMax, 0, 0);
                newRoomPosition -= new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (door.direction == Direction.Right)
            {
                newRoomPosition += new Vector3Int(initialRoom.roomBorders.xMax, 0, 0);
                newRoomPosition += new Vector3Int(-roomToConnect.roomBorders.xMin, 0, 0);
                newRoomPosition += new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (door.direction == Direction.Up)
            {
                newRoomPosition += new Vector3Int(0, initialRoom.roomBorders.yMax, 0);
                newRoomPosition += new Vector3Int(0, -roomToConnect.roomBorders.yMin, 0);
                newRoomPosition += new Vector3Int(randomOffset, additionalDistance, 0);
            }

            // Find new position
            Vector3Int newPosition = initialRoom.globalPosition + newRoomPosition;
            Room newRoom = Instantiate(roomToConnect, newPosition, Quaternion.identity, parentFolder);
            newRoom.globalPosition = newPosition;

            if (RoomInteractsWithPlacedRooms(newRoom, additionalDistance))
            {
                Destroy(newRoom.gameObject);
                continue;
            }

            initialRoom.SetDoorConnected(door);
            Door newRoomDoor = newRoom.GetDoorByDirection(door.GetOppositeDirection());

            newRoom.previousRoom = initialRoom;
            newRoom.SetDoorConnected(newRoomDoor);
            newRoom.DrawRoom();

            Door initialDoor = door + initialRoom.globalPosition;
            Corridor corridor = new Corridor();
            corridor.DrawCorridor(initialDoor, newRoomDoor.position + newRoom.globalPosition);

            placedRooms.Add(newRoom);
            count++;

            yield return null;
        }

        foreach (Room room in placedRooms)
        {
            room.OpenDoors();
            room.AddDoorTriggers();
        }

        // Spawn patrick in a random room
        Room roomToSpawnPatrick = placedRooms[placedRooms.Count - 1];
        roomToSpawnPatrick.spawnPatrick = true;

        Debug.Log("Dungeon generation time: " + (Time.time - startTime));
    }

    private Room GetRoomByDirection(Direction direction, System.Random rand)
    {
        if (roomsByDirection[direction].Count != 0)
        {
            return roomsByDirection[direction][rand.Next(0, roomsByDirection[direction].Count)];
        }

        return null;
    }

    private Room GetAndRemoveStartingRoom()
    {
        Room startingRoom = null;

        foreach (Room room in availableRooms)
        {
            if (room.name == "start")
            {
                startingRoom = room;
                break;
            }
        }

        availableRooms.Remove(startingRoom);
        return startingRoom;
    }

    private bool RoomInteractsWithPlacedRooms(Room placedRoom, int additionalDistance)
    {
        foreach (Room roomToCheck in placedRooms)
        {
            // Check if either room is completely left of the other
            if (
                placedRoom.globalPosition.x + placedRoom.roomBorders.xMin - additionalDistance >= roomToCheck.globalPosition.x + roomToCheck.roomBorders.xMax ||
                roomToCheck.globalPosition.x + roomToCheck.roomBorders.xMin - additionalDistance >= placedRoom.globalPosition.x + placedRoom.roomBorders.xMax)
            {
                continue;
            }

            // Check if either room is completely above the other
            if (
                placedRoom.globalPosition.y + placedRoom.roomBorders.yMin - additionalDistance >= roomToCheck.globalPosition.y + roomToCheck.roomBorders.yMax ||
                roomToCheck.globalPosition.y + roomToCheck.roomBorders.yMin - additionalDistance >= placedRoom.globalPosition.y + placedRoom.roomBorders.yMax)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    private void LoadAllRoomsInResources()
    {
        string resourceFolder = "Rooms";

        UnityEngine.Object[] rooms = Resources.LoadAll(resourceFolder, typeof(Room));
        roomsByDirection.Add(Direction.Down, new List<Room>());
        roomsByDirection.Add(Direction.Up, new List<Room>());
        roomsByDirection.Add(Direction.Left, new List<Room>());
        roomsByDirection.Add(Direction.Right, new List<Room>());

        foreach (Room room in rooms)
        {
            foreach (Door door in room.doors)
            {
                if (!roomsByDirection[door.direction].Contains(room))
                {
                    roomsByDirection[door.direction].Add(room);
                }
            }

            availableRooms.Add(room);
        }
    }
}
