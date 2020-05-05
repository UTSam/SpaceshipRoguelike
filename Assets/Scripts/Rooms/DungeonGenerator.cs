using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public Room[] availableRooms;
    public int roomCount = 10;
    public int seed;
    public int additionalDistance = 10;
    public int maxOffset = 20;

    private List<Room> placedRooms = new List<Room>();

    public void Start()
    {
        StartCoroutine("GenerateDungeon");
    }

    IEnumerator GenerateDungeon()
    {
        System.Random rand = new System.Random(seed);

        int count = 0;

        // Place starting room
        int roomIndex = rand.Next(availableRooms.Length);
        Room spawnRoom = Instantiate(availableRooms[roomIndex], this.transform);
        placedRooms.Add(spawnRoom);

        // Keep placing rooms until roomCount
        while (count < roomCount)
        {
            // Pick random room from placed rooms
            int i = rand.Next(placedRooms.Count);
            Room toAddTo = placedRooms[i];

            // Pick random room to add
            Room toAdd = availableRooms[rand.Next(availableRooms.Length)];

            // Pick random dir
            Vector2Int[] directions =
            {
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right,
                Vector2Int.up
            };
            Vector2Int dir = directions[rand.Next(directions.Length)];

            int whatever = rand.Next(maxOffset) - (maxOffset / 2);

            // Get right distance from dir
            if (dir == Vector2Int.down)
            {
                dir -= new Vector2Int(0, -toAddTo.roomBorders.yMin);
                dir -= new Vector2Int(0, toAdd.roomBorders.yMax);
                dir -= new Vector2Int(whatever, additionalDistance);
            }
            else if (dir == Vector2Int.left)
            {
                dir -= new Vector2Int(-toAddTo.roomBorders.xMin, 0);
                dir -= new Vector2Int(toAdd.roomBorders.xMax, 0);
                dir -= new Vector2Int(additionalDistance, whatever);
            }
            else if (dir == Vector2Int.right)
            {
                dir += new Vector2Int(toAddTo.roomBorders.xMax, 0);
                dir += new Vector2Int(-toAdd.roomBorders.xMin, 0);
                dir += new Vector2Int(additionalDistance, whatever);
            }
            else if (dir == Vector2Int.up)
            {
                dir += new Vector2Int(0, toAddTo.roomBorders.yMax);
                dir += new Vector2Int(0, -toAdd.roomBorders.yMin);
                dir += new Vector2Int(whatever, additionalDistance);
            }

            // Find new pos
            Vector3Int newPos = toAddTo.pos + new Vector3Int(dir.x, dir.y, 0);
            Room newRoom = Instantiate(toAdd, newPos, Quaternion.identity, this.transform);
            newRoom.pos = newPos;

            // Check if toAdd intersects with another room
            bool intersects = false;
            foreach (Room r in placedRooms)
            {
                if (roomsIntersect(newRoom, r, additionalDistance))
                {
                    intersects = true;
                }
            }

            if (intersects)
            {
                Destroy(newRoom.gameObject);
            }
            else
            {
                placedRooms.Add(newRoom);
                count++;
            }

            yield return null;

        }

        // Draw all rooms
        foreach (Room r in placedRooms)
        {
            r.DrawRoom();
        }
    }

    private bool roomsIntersect(Room r1, Room r2, int additionalDistance)
    {
        // Check if either room is completely left of the other
        if (
            r1.pos.x + r1.roomBorders.xMin - additionalDistance >= r2.pos.x + r2.roomBorders.xMax ||
            r2.pos.x + r2.roomBorders.xMin - additionalDistance >= r1.pos.x + r1.roomBorders.xMax)
        {
            return false;
        }

        // Check if either room is completely above the other
        if (
            r1.pos.y + r1.roomBorders.yMin - additionalDistance >= r2.pos.y + r2.roomBorders.yMax ||
            r2.pos.y + r2.roomBorders.yMin - additionalDistance >= r1.pos.y + r1.roomBorders.yMax)
        {
            return false;
        }

        return true;
    }
}
