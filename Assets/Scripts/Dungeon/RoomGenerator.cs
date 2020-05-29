using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Scripts.Dungeon;

public class RoomGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile doorLeft;
    public Tile doorRight;
    public Tile doorUp;
    public Tile doorDown;

    Room.RoomBorders borders;

    public GameObject GetGameObject()
    {
        return GetRoom().gameObject;
    }

    public Room GetRoom()
    {
        Assets.Scripts.Dungeon.TileData tilesWalls = new Assets.Scripts.Dungeon.TileData("Walls");
        Assets.Scripts.Dungeon.TileData tilesFloor = new Assets.Scripts.Dungeon.TileData("Floors");
        Assets.Scripts.Dungeon.TileData tilesDoors = new Assets.Scripts.Dungeon.TileData("Doors");

        List<Door> doors = new List<Door>();

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

                if (tile == null) continue;

                // Check for door
                if (tile.name.StartsWith("DoorIndicator"))
                {
                    Door newDoor = new Door(tilePos);
                    switch(tile.name)
                    {
                        case "DoorIndicator_Up":
                            newDoor.direction = Direction.Up;
                            tile = GVC.Instance.tiles.corridorHorizontal;
                            break;
                        case "DoorIndicator_Down":
                            newDoor.direction = Direction.Down;
                            tile = GVC.Instance.tiles.corridorHorizontal;
                            break;
                        case "DoorIndicator_Left":
                            newDoor.direction = Direction.Left;
                            tile = GVC.Instance.tiles.corridorVertical;
                            break;
                        case "DoorIndicator_Right":
                            newDoor.direction = Direction.Right;
                            tile = GVC.Instance.tiles.corridorVertical;
                            break;
                    }
                    doors.Add(newDoor);

                    tilesDoors.tiles.Add(tile);
                    tilesDoors.tileLocalPositions.Add(tilePos);
                }

                // Check for door
                if (tile.name.StartsWith("Floor"))
                {
                    tilesFloor.tiles.Add(tile);
                    tilesFloor.tileLocalPositions.Add(tilePos);
                }

                // Check for door
                if (tile.name.StartsWith("Wall"))
                {
                    tilesWalls.tiles.Add(tile);
                    tilesWalls.tileLocalPositions.Add(tilePos);
                }

                // Update borders
                if (x < borders.xMin) borders.xMin = x;
                if (y < borders.yMin) borders.yMin = y;
                if (x > borders.xMax) borders.xMax = x;
                if (y > borders.yMax) borders.yMax = y;
            }
        }

        // Because of tiles, we need to up the max X and Y 
        borders.xMax++;
        borders.yMax++;
        
        // Create empty gameobject, add "Room" script and populate values
        GameObject newObj = new GameObject("Room");
        Room room = newObj.AddComponent<Room>();

        room.tileDataArray.Add(tilesWalls);
        room.tileDataArray.Add(tilesFloor);
        room.tileDataArray.Add(tilesDoors);

        room.roomBorders = borders;
        room.doors = doors; 
        room = ResetRoomToCenter(room);

        return room;
    }

    // Set the position of the room to the center
    private Room ResetRoomToCenter(Room room)
    {
        Room.RoomBorders rb = room.roomBorders;
        int dx = -(rb.xMax + rb.xMin) / 2;
        int dy = -(rb.yMax + rb.yMin) / 2;

        foreach (Assets.Scripts.Dungeon.TileData pls in room.tileDataArray)
        {
            for (int i = 0; i < pls.tileLocalPositions.Count; i++)
            {
                pls.tileLocalPositions[i] += new Vector3Int(dx, dy, 0);
            }
        }

        // Change room borders
        rb.xMin += dx;
        rb.xMax += dx;
        rb.yMin += dy;
        rb.yMax += dy;

        // Change door positions
        foreach (Door door in room.doors)
        {
            door.position += new Vector3Int(dx, dy, 0);
        }

        room.roomBorders = rb;

        return room;
    }
}
