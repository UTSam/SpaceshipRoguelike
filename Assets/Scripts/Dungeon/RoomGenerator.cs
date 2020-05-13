using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Assets.Scripts.Rooms;

public struct RectSimpl
{
    public int width;
    public int height;

    public RectSimpl(int _width, int _height)
    {
        width = _width;
        height = _height;
    }
}

public class RoomGenerator : MonoBehaviour
{
    Room.RoomBorders borders;

    public GameObject GetGameObject()
    {
        return GetRoom().gameObject;
    }

    public Room GetRoom()
    {
        Tilemap tilemap = DungeonManager.tilemap_walls;

        List<Tile> tiles = new List<Tile>();
        List<Door> doors = new List<Door>();
        List<Vector3Int> pos = new List<Vector3Int>();

        // Reset borders
        borders.xMin = int.MaxValue;
        borders.yMin = int.MaxValue;
        borders.xMax = int.MinValue;
        borders.yMax = int.MinValue;

        // Add tiles to list
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Tile tile = tilemap.GetTile<Tile>(tilePos);

                if (tile != null)
                {
                    // Check for door
                    if (tile.name.StartsWith("DoorIndicator"))
                    {
                        Debug.Log(tile.name);
                        Door newDoor = new Door(tilePos);
                        switch(tile.name)
                        {
                            case "DoorIndicator_Up":
                                newDoor.Direction = Direction.Up;
                                break;
                            case "DoorIndicator_Down":
                                newDoor.Direction = Direction.Down;
                                break;
                            case "DoorIndicator_Left":
                                newDoor.Direction = Direction.Left;
                                break;
                            case "DoorIndicator_Right":
                                newDoor.Direction = Direction.Right;
                                break;
                        }
                        doors.Add(newDoor);
                    }

                    tiles.Add(tile);
                    pos.Add(tilePos);

                    // Update borders
                    if (x < borders.xMin) borders.xMin = x;
                    if (y < borders.yMin) borders.yMin = y;
                    if (x > borders.xMax) borders.xMax = x;
                    if (y > borders.yMax) borders.yMax = y;
                }
            }
        }

        // Because of tiles, we need to up the max X and Y 
        borders.xMax++;
        borders.yMax++;
        
        // Create empty gameobject, add "Room" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>();
        room.tiles = tiles.ToArray();
        room.tilePositions = pos.ToArray();
        room.roomBorders = borders;
        room.doors = doors; 
        room = ResetRoomToCenter(room);

        return room;
    }

    // Set the position of the room to the center
    private Room ResetRoomToCenter(Room room)
    {
        Vector3Int[] tilePositions = room.tilePositions;
        Room.RoomBorders rb = room.roomBorders;

        int dx = -(rb.xMax + rb.xMin) / 2;
        int dy = -(rb.yMax + rb.yMin) / 2;

        // Change tile positions
        for (int i = 0; i < tilePositions.Length; i++)
        {
            tilePositions[i].x += dx; 
            tilePositions[i].y += dy; 
        }

        // Change room borders
        rb.xMin += dx;
        rb.xMax += dx;
        rb.yMin += dy;
        rb.yMax += dy; 

        // Change door positions
        foreach (Door door in room.doors)
        {
            door.Position += new Vector3Int(dx, dy, 0);
        }


        room.tilePositions = tilePositions;
        room.roomBorders = rb; 

        return room;
    }
}
