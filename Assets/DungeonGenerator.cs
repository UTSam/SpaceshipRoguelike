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
        startingRoom.SetPosition(Vector2Int.zero);
        startingRoom.DrawRoom();

        RecursiveMakeRooms(startingRoom);
    }

    void RecursiveMakeRooms(Room room)
    {
        foreach (Door door in room.GetDoorPositions())
        {
            if (currentRooms >= maxRooms)
                return;

            Room spawnedRoom = null;
            Vector2Int addToPosition = Vector2Int.zero;
            int paddingBetweenRooms = 10;

            if (door.Direction == direction.Down)
            {
                if (RoomsEntranceUp.Length == 0)
                    continue;

                spawnedRoom = Instantiate(RoomsEntranceUp[Random.Range(0, RoomsEntranceUp.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.y = -(spawnedRoom.height + paddingBetweenRooms);
                spawnedRoom.RemoveDoor(direction.Up);
            }

            if (door.Direction == direction.Up)
            {
                if (RoomsEntranceDown.Length == 0)
                    continue;

                spawnedRoom = Instantiate(RoomsEntranceDown[Random.Range(0, RoomsEntranceDown.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.y = room.height + paddingBetweenRooms;
                spawnedRoom.RemoveDoor(direction.Down);
            }

            if (door.Direction == direction.Left)
            {
                if (RoomsEntranceRight.Length == 0)
                    continue;

                spawnedRoom = Instantiate(RoomsEntranceRight[Random.Range(0, RoomsEntranceRight.Length)], Vector3Int.zero, Quaternion.identity) as Room;
                addToPosition.x = -(spawnedRoom.width + paddingBetweenRooms);
                spawnedRoom.RemoveDoor(direction.Right);
            }

            if (door.Direction == direction.Right)
            {
                if (RoomsEntranceLeft.Length == 0)
                    continue;

                spawnedRoom = Instantiate(RoomsEntranceLeft[Random.Range(0, RoomsEntranceLeft.Length)], Vector3Int.zero, Quaternion.identity) as Room;
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
