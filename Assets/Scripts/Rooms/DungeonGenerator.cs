using Assets.Scripts.Rooms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private List<Room> availableRooms;

    public int count = 0;
    public int roomCount = 10;
    public int seed;
    public int additionalDistance = 10;
    public int maxOffset = 20;
    readonly Vector3Int[] directions =
    {
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up
    };

    private List<Room> placedRooms = new List<Room>();

    public void Start()
    {
        availableRooms = LoadAllPrefabsOfType<Room>("Assets/Rooms");

        StartCoroutine("GenerateDungeon");
    }

    IEnumerator GenerateDungeon()
    {
        System.Random rand = new System.Random(seed);

        // Place starting room
        Room spawnRoom = Instantiate(GetAndRemoveStartingRoom(), this.transform);
        placedRooms.Add(spawnRoom);

        // Keep placing rooms until roomCount
        while (count < roomCount)
        {
            // Pick random room from placed rooms
            Room toAddTo = placedRooms[rand.Next(placedRooms.Count)];

            // Pick random room to add
            Room toAdd = availableRooms[rand.Next(availableRooms.Count)];

            // Pick random direction
            Vector3Int newRoomPosition = directions[rand.Next(directions.Length)];

            int randomOffset = rand.Next(maxOffset * 2) - maxOffset;

            // Get right distance from direction
            if (newRoomPosition == Vector3Int.down)
            {
                newRoomPosition -= new Vector3Int(0, -toAddTo.roomBorders.yMin, 0);
                newRoomPosition -= new Vector3Int(0, toAdd.roomBorders.yMax, 0);
                newRoomPosition -= new Vector3Int(randomOffset, additionalDistance, 0);
            }
            else if (newRoomPosition == Vector3Int.left)
            {
                newRoomPosition -= new Vector3Int(-toAddTo.roomBorders.xMin, 0, 0);
                newRoomPosition -= new Vector3Int(toAdd.roomBorders.xMax, 0, 0);
                newRoomPosition -= new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (newRoomPosition == Vector3Int.right)
            {
                newRoomPosition += new Vector3Int(toAddTo.roomBorders.xMax, 0, 0);
                newRoomPosition += new Vector3Int(-toAdd.roomBorders.xMin, 0, 0);
                newRoomPosition += new Vector3Int(additionalDistance, randomOffset, 0);
            }
            else if (newRoomPosition == Vector3Int.up)
            {
                newRoomPosition += new Vector3Int(0, toAddTo.roomBorders.yMax, 0);
                newRoomPosition += new Vector3Int(0, -toAdd.roomBorders.yMin, 0);
                newRoomPosition += new Vector3Int(randomOffset, additionalDistance, 0);
            }

            // Find new position
            Vector3Int newPos = toAddTo.position + newRoomPosition;
            Room newRoom = Instantiate(toAdd, newPos, Quaternion.identity, this.transform);
            newRoom.position = newPos;

            if (RoomInteractsWithPlacedRooms(newRoom, additionalDistance))
            {
                Destroy(newRoom.gameObject);
            }
            else
            {
                placedRooms.Add(newRoom);
                newRoom.previousRoom = toAddTo;
                count++;
            }

            yield return null;
        }

        // Draw all rooms
        foreach (Room r in placedRooms)
        {
            r.DrawRoom();

            foreach (Door door in r.doors)
                Debug.Log(door.Position);
        }
    }
    private bool RoomInteractsWithPlacedRooms(Room placedRoom, int additionalDistance)
    {
        foreach (Room roomToCheck in placedRooms)
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

    private static List<T> LoadAllPrefabsOfType<T>(string path) where T : MonoBehaviour
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

        //loop through directory loading the game object and checking if it has the component you want
        List<T> prefabComponents = new List<T>();
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

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

    private Room GetAndRemoveStartingRoom()
    {
        Room startingRoom = null;

        foreach (Room room in availableRooms)
        {
            if (room.name == "start")
            {
                startingRoom = room;
            }
        }

        availableRooms.Remove(startingRoom);
        return startingRoom;
    }
}
