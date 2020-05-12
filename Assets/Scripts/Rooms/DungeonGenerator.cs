using Assets.Scripts.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]

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
                PlacedRooms.Add(newRoom);
                newRoom.previousRoom = initialRoom;
                initialRoom.RemoveDoor(door);
                newRoom.RemoveDoor(newRoom.GetDoorByDirection(door.GetOppositeDirection()));
                count++;
            }

            yield return null;
        }

        // Draw all rooms
        foreach (Room r in PlacedRooms)
        {
            r.DrawRoom();
        }

        Debug.Log("Dungeon generation time: " + (Time.time - startTime));
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
            Debug.Log(prefab);
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
