using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Room startingRoom;
    public Room[] roomsEntranceDown;
    public Room[] roomsEntranceUp;
    public Room[] roomsEntranceLeft;
    public Room[] roomsEntranceRight;

    public int paddingBetweenRooms = 10;
    private int maxRooms = 8;
    private int currentRooms = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<Door> AvailableSpawnLocations = new List<Door>();

        Room newStartingRoom = Instantiate(startingRoom, new Vector3Int(0, 0, 0), Quaternion.identity) as Room;
        newStartingRoom.SetPosition(Vector2Int.zero);
        newStartingRoom.DrawRoom();

        RecursiveMakeRooms(newStartingRoom);
    }

    void RecursiveMakeRooms(Room room)
    {
        foreach (Door door in room.GetDoorPositions())
        {
            if (currentRooms >= maxRooms)
                return;

            Room spawnedRoom = null;
            Vector2Int addToPosition = Vector2Int.zero;

            if (door.Direction == direction.Down)
            {
                if (roomsEntranceUp.Length == 0)
                    continue;

                spawnedRoom = Instantiate(roomsEntranceUp[Random.Range(0, roomsEntranceUp.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.y = -(spawnedRoom.height + paddingBetweenRooms);
                spawnedRoom.RemoveDoor(direction.Up);
            }

            if (door.Direction == direction.Up)
            {
                if (roomsEntranceDown.Length == 0)
                    continue;

                spawnedRoom = Instantiate(roomsEntranceDown[Random.Range(0, roomsEntranceDown.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.y = room.height + paddingBetweenRooms;
                spawnedRoom.RemoveDoor(direction.Down);
            }

            if (door.Direction == direction.Left)
            {
                if (roomsEntranceRight.Length == 0)
                    continue;

                spawnedRoom = Instantiate(roomsEntranceRight[Random.Range(0, roomsEntranceRight.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.x = -(spawnedRoom.width + paddingBetweenRooms);
                spawnedRoom.RemoveDoor(direction.Right);
            }

            if (door.Direction == direction.Right)
            {
                if (roomsEntranceLeft.Length == 0)
                    continue;

                spawnedRoom = Instantiate(roomsEntranceLeft[Random.Range(0, roomsEntranceLeft.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.x = room.width + paddingBetweenRooms;
                spawnedRoom.RemoveDoor(direction.Left);
            }

            spawnedRoom.name = room.name + "-" + door.Direction;
            spawnedRoom.SetPosition(room.offsetPosition + addToPosition);
            spawnedRoom.DrawRoom();

            currentRooms++;
            RecursiveMakeRooms(spawnedRoom);
        }
    }
}
