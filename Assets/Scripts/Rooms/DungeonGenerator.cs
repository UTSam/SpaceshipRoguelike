﻿using Assets.Scripts.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    private List<Room> availableRooms = new List<Room>();
    private List<Room> PlacedRooms = new List<Room>();

    private Dictionary<Direction, List<Room>> RoomsByDirection = new Dictionary<Direction, List<Room>>();
    
    public int count = 0;
    public int roomCount = 50;
    public int seed;
    public int additionalDistance = 10;
    public int maxOffset = 20;

    public Tilemap tilemap;
    public Tile corridorTile;
    public Tile wallTile;

    private float startTime;

    public void Start()
    {
        availableRooms = LoadAllPrefabsInResourcesOfType<Room>("Rooms");
        FillEntranceRoomsLists();

        startTime = Time.time;
        StartCoroutine("GenerateDungeon");
    }

    IEnumerator GenerateDungeon()
    {
        System.Random rand = new System.Random(seed);

        // Place starting room
        Room spawnRoom = Instantiate(GetAndRemoveStartingRoom(), this.transform);
        PlacedRooms.Add(spawnRoom);
        spawnRoom.DrawRoom();

        // Keep placing rooms until roomCount
        while (count < roomCount)
        {
            // Pick random room from placed rooms
            Room initialRoom = PlacedRooms[rand.Next(PlacedRooms.Count)];
            if (initialRoom.doors.Count == 0)
                continue;

            // Get a room based on the direcion of the door
            Door door = initialRoom.GetRandomDoor(rand);
            if (door == null)
                continue;

            Room roomToConnect = GetRoomByDirection(door.GetOppositeDirection(), rand);
            if(roomToConnect == null)
                continue;

            int randomOffset = rand.Next(maxOffset * 2) - maxOffset;
            Vector3Int newRoomPosition = Vector3Int.zero;
            if (door.Direction == Direction.Down)
            {
                newRoomPosition -= new Vector3Int(0, -initialRoom.roomBorders.yMin, 0);
                newRoomPosition -= new Vector3Int(0, roomToConnect.roomBorders.yMax, 0);
                newRoomPosition -= new Vector3Int(randomOffset, additionalDistance, 0);
            }
            else if (door.Direction == Direction.Left)
            {
                newRoomPosition -= new Vector3Int(-initialRoom.roomBorders.xMin, 0, 0);
                newRoomPosition -= new Vector3Int(roomToConnect.roomBorders.xMax, 0, 0);
                newRoomPosition -= new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (door.Direction == Direction.Right)
            {
                newRoomPosition += new Vector3Int(initialRoom.roomBorders.xMax, 0, 0);
                newRoomPosition += new Vector3Int(-roomToConnect.roomBorders.xMin, 0, 0);
                newRoomPosition += new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (door.Direction == Direction.Up)
            {
                newRoomPosition += new Vector3Int(0, initialRoom.roomBorders.yMax, 0);
                newRoomPosition += new Vector3Int(0, -roomToConnect.roomBorders.yMin, 0);
                newRoomPosition += new Vector3Int(randomOffset, additionalDistance, 0);
            }

            // Find new position
            Vector3Int newPosition = initialRoom.position + newRoomPosition;
            Room newRoom = Instantiate(roomToConnect, newPosition, Quaternion.identity, this.transform);
            newRoom.position = newPosition;

            if (RoomInteractsWithPlacedRooms(newRoom, additionalDistance))
            {
                Destroy(newRoom.gameObject);
            }
            else
            {
                Door newRoomDoor = newRoom.GetDoorByDirection(door.GetOppositeDirection());

                PlacedRooms.Add(newRoom);
                newRoom.previousRoom = initialRoom;
                initialRoom.DoorIsConnected(door);
                newRoom.DoorIsConnected(newRoomDoor);
                count++;
                newRoom.DrawRoom();

                door.Position += initialRoom.position;
                CreateCorridor(door, newRoomDoor.Position + newRoom.position);
            }

            yield return null;
        }

        Debug.Log("Dungeon generation time: " + (Time.time - startTime));
    }

    #region Spawn Tile functions
    private void SpawnCorridorTile(Vector3Int position)
    {
        tilemap.SetTile(position, corridorTile);
    }

    private void SpawnWallTile(Vector3Int position)
    {
        tilemap.SetTile(position, wallTile);
    }

    private void SpawnHorizontalCorridor(Vector3Int currentCorridorPosition)
    {
        SpawnWallTile    (new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 2, 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 1, 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y    , 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 1, 0));
        SpawnWallTile    (new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 2, 0));
    }

    private void SpawnVerticalCorridor(Vector3Int currentCorridorPosition)
    {
        SpawnWallTile    (new Vector3Int(currentCorridorPosition.x - 2, currentCorridorPosition.y, 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x - 1, currentCorridorPosition.y, 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x    , currentCorridorPosition.y, 0));
        SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x + 1, currentCorridorPosition.y, 0));
        SpawnWallTile    (new Vector3Int(currentCorridorPosition.x + 2, currentCorridorPosition.y, 0));
    }

    private void SpawnVerticalWalls(Vector3Int currentCorridorPosition)
    {
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 2, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 1, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y    , 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 1, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 2, 0));
    }
    private void SpawnHorizontalWalls(Vector3Int currentCorridorPosition)
    {
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x - 2, currentCorridorPosition.y, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x - 1, currentCorridorPosition.y, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x    , currentCorridorPosition.y, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x + 1, currentCorridorPosition.y, 0));
        SpawnWallTile(new Vector3Int(currentCorridorPosition.x + 2, currentCorridorPosition.y, 0));
    }
    #endregion

    private void CreateCorridor(Door door, Vector3Int connectedDoorPosition)
    {
        Vector3Int difference = -door.Position + connectedDoorPosition;
        Vector3Int differenceAbs = new Vector3Int(Math.Abs(difference.x), Math.Abs(difference.y), 0);
        Vector3Int currentCorridorPosition = door.Position;

        int signX = 0;
        if (difference.x != 0 && differenceAbs.x != 0)
            signX = difference.x / differenceAbs.x;

        int signY = 0;
        if (difference.y != 0 && differenceAbs.y != 0)
            signY = difference.y / differenceAbs.y;

        if (door.Direction == Direction.Right || door.Direction == Direction.Left)
        {
            int corridorHorizontalLengthHalf = differenceAbs.x / 2;
            int corridorVerticalLength = differenceAbs.y;

            // Go horizontal for half the way
            for (int j = 0; j < corridorHorizontalLengthHalf; j++)
            {
                currentCorridorPosition.x += signX;
                SpawnHorizontalCorridor(currentCorridorPosition);
            }

            // Move 1 more since the corridor is 3 width
            SpawnHorizontalCorridor(new Vector3Int(currentCorridorPosition.x + signX, currentCorridorPosition.y, 0));
            SpawnVerticalWalls(new Vector3Int(currentCorridorPosition.x + signX * 2, currentCorridorPosition.y, 0));

            // Go vertical
            for (int j = 0; j < corridorVerticalLength; j++)
            {
                currentCorridorPosition.y += signY;
                SpawnVerticalCorridor(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + signY, 0));// The plus signY since we don't want it to spawn in the already spawned corridor.
            }

            if(corridorVerticalLength != 0)
                SpawnHorizontalWalls(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + signY * 2, 0));

            // Go horizontal for half the way
            currentCorridorPosition.x += signX;
            for (int j = 0; j < differenceAbs.x - corridorHorizontalLengthHalf - 1; j++)
            {
                currentCorridorPosition.x += signX;
                if (currentCorridorPosition != connectedDoorPosition)
                    SpawnHorizontalCorridor(currentCorridorPosition);
            }
        }

        if (door.Direction == Direction.Up || door.Direction == Direction.Down)
        {
            //Debug.Log(corridorHorizontalLength);
            int corridorVerticalLengthHalf = differenceAbs.y / 2;
            int corridorHorizontalLength = differenceAbs.x;

            // go vertical half the way
            for (int i = 0; i < corridorVerticalLengthHalf; i++)
            {
                currentCorridorPosition.y += signY;
                SpawnVerticalCorridor(currentCorridorPosition);
            }

            // Move 1 more since the corridor is 3 width
            SpawnVerticalCorridor(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + signY, 0));
            SpawnHorizontalWalls(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + signY * 2, 0));

            // go horizontal
            for (int i = 0; i < corridorHorizontalLength; i++)
            {
                currentCorridorPosition.x += signX;
                SpawnHorizontalCorridor(new Vector3Int(currentCorridorPosition.x + signX, currentCorridorPosition.y, 0));// The plus signX since we don't want it to spawn in the already spawned corridor.
            }

            if (corridorHorizontalLength != 0)
                SpawnVerticalWalls(new Vector3Int(currentCorridorPosition.x + signX * 2, currentCorridorPosition.y, 0));

            // go vertical half the way
            currentCorridorPosition.y += signY;
            for (int i = 0; i < differenceAbs.y - corridorVerticalLengthHalf - 1; i++)
            {
                currentCorridorPosition.y += signY;
                if (currentCorridorPosition != connectedDoorPosition)
                    SpawnVerticalCorridor(currentCorridorPosition);
            }
        }

        // Remove the doors
        tilemap.SetTile(door.Position, null);
        tilemap.SetTile(connectedDoorPosition, null);
    }

    private Room GetRoomByDirection(Direction direction, System.Random rand)
    {
        if(RoomsByDirection[direction].Count != 0)
        {
            return RoomsByDirection[direction][rand.Next(0, RoomsByDirection[direction].Count)];
        }

        return null;
    }

    private void FillEntranceRoomsLists()
    {
        RoomsByDirection.Add(Direction.Down, new List<Room>());
        RoomsByDirection.Add(Direction.Up, new List<Room>());
        RoomsByDirection.Add(Direction.Left, new List<Room>());
        RoomsByDirection.Add(Direction.Right, new List<Room>());

        foreach (Room room in availableRooms)
        {
            room.SetDoors();
            foreach (Door door in room.doors)
            {
                if (!RoomsByDirection[door.Direction].Contains(room))
                {
                    RoomsByDirection[door.Direction].Add(room);
                }
            }
        }
    }

    private Room GetAndRemoveStartingRoom()
    {
        Room startingRoom = null;

        foreach (Room room in availableRooms)
        {
            // For some reason unity doesn't save the variables or whatever.
            if (room.name == "start")
            {
                startingRoom = room;
            }
        }

        availableRooms.Remove(startingRoom);
        return startingRoom;
    }

    private bool RoomInteractsWithPlacedRooms(Room placedRoom, int additionalDistance)
    {
        foreach (Room roomToCheck in PlacedRooms)
        {
            // Check if either room is completely left of the other
            if (
                placedRoom.position.x + placedRoom.roomBorders.xMin - additionalDistance >= roomToCheck.position.x + roomToCheck.roomBorders.xMax ||
                roomToCheck.position.x + roomToCheck.roomBorders.xMin - additionalDistance >= placedRoom.position.x + placedRoom.roomBorders.xMax)
            {
                continue;
            }

            // Check if either room is completely above the other
            if (
                placedRoom.position.y + placedRoom.roomBorders.yMin - additionalDistance >= roomToCheck.position.y + roomToCheck.roomBorders.yMax ||
                roomToCheck.position.y + roomToCheck.roomBorders.yMin - additionalDistance >= placedRoom.position.y + placedRoom.roomBorders.yMax)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    private List<T> LoadAllPrefabsInResourcesOfType<T>(string path) where T : MonoBehaviour
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo("Assets/Resources/" + path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

        //loop through directory loading the game object and checking if it has the component you want
        List<T> prefabComponents = new List<T>();
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            GameObject prefab = Resources.Load<GameObject>(path + "/" + RemoveFileExtension(fileInfo.Name));
            if (prefab != null)
            {
                T hasT = prefab.GetComponent<T>();
                if (hasT != null)
                {
                    prefabComponents.Add(hasT);
                }
            }
        }
        return prefabComponents;
    }

    public string RemoveFileExtension(string fileName)
    {
        string filenameWithoutExt = "";
        int fileExtPos = fileName.LastIndexOf(".", StringComparison.Ordinal);

        if (fileExtPos >= 0)
            filenameWithoutExt = fileName.Substring(0, fileExtPos);

        return filenameWithoutExt;
    }
}
