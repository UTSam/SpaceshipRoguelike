using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Room StartingRoom;
    public Room[] RoomsEntranceDown;
    public Room[] RoomsEntranceUp;
    public Room[] RoomsEntranceLeft;
    public Room[] RoomsEntranceRight;

    private int maxRooms = 8;
    private int currentRooms = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<Door> AvailableSpawnLocations = new List<Door>();

        Room startingRoom = Instantiate(StartingRoom, new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
        startingRoom.SetPosition(Vector3Int.zero);
        startingRoom.CreateRoom();
        startingRoom.name = "StartingRoom";

        RecursiveMakeRooms(startingRoom);
    }

    void RecursiveMakeRooms(Room room)
    {
        foreach (Door door in room.GetDoorPositions())
        {
            if (currentRooms >= maxRooms)
                return;

            Vector3Int currentPosition = room.offsetPosition;

            Vector3Int addToPosition = Vector3Int.zero;
            Room spawnedRoom = null;

            if (door.Direction == direction.Down)
            {
                spawnedRoom = Instantiate(RoomsEntranceUp[Random.Range(0, RoomsEntranceUp.Length)], new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
                addToPosition = new Vector3Int(0, -(room.height + 15), 0);
                spawnedRoom.RemoveDoor(direction.Up);
            }

            if (door.Direction == direction.Up)
            {
                spawnedRoom = Instantiate(RoomsEntranceDown[Random.Range(0, RoomsEntranceDown.Length)], new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
                addToPosition = new Vector3Int(0, room.height + 15, 0);
                spawnedRoom.RemoveDoor(direction.Down);
            }

            if (door.Direction == direction.Left)
            {
                spawnedRoom = Instantiate(RoomsEntranceRight[Random.Range(0, RoomsEntranceRight.Length)], new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
                addToPosition = new Vector3Int(-(room.width + 15), 0, 0);
                spawnedRoom.RemoveDoor(direction.Right);
            }

            if (door.Direction == direction.Right)
            {
                spawnedRoom = Instantiate(RoomsEntranceLeft[Random.Range(0, RoomsEntranceLeft.Length)], new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
                addToPosition = new Vector3Int(room.width + 15, 0, 0);
                spawnedRoom.RemoveDoor(direction.Left);
            }

            spawnedRoom.name = room.name + "-" + door.Direction;

            spawnedRoom.SetPosition(currentPosition + addToPosition);
            spawnedRoom.CreateRoom();
            currentRooms++;

            RecursiveMakeRooms(spawnedRoom);
        }
    }
}
