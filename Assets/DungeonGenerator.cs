using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public Room StartingRoom;

    public Room[] rooms;

    private List<Vector3Int> availableSpawnLocations = new List<Vector3Int>();
    // Start is called before the first frame update
    void Start()
    {
        Room spawnedRoom = Instantiate(StartingRoom, new Vector3Int(0, 0, 0), Quaternion.identity) as Room;

        availableSpawnLocations.AddRange(spawnedRoom.GetDoorPositions());

        foreach (var item in availableSpawnLocations)
        {
            Debug.Log(item);
        }
        spawnedRoom.CreateRoom(Vector3Int.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
