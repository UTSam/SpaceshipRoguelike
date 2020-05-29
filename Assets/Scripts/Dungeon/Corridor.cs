using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon
{
    public class Corridor
    {
        public List<Door> connectedDoors = new List<Door>();

        public TileData tileData = new TileData("Walls");

        #region Spawn Tile functions
        private void SpawnCorridorTile(Vector3Int position)
        {
            GVC.Instance.tilemap.walls.SetTile(position, null);
            GVC.Instance.tilemap.floor.SetTile(position, GVC.Instance.tiles.floor);
        }

        private void SpawnWallHorizontalTile(Vector3Int position)
        {
            Tile tile = GVC.Instance.tiles.corridorHorizontal;
            tileData.AddTile(tile, position);
            GVC.Instance.tilemap.walls.SetTile(position, tile);
        }

        private void SpawnWallVerticalTile(Vector3Int position)
        {
            Tile tile = GVC.Instance.tiles.corridorVertical;
            tileData.AddTile(tile, position);
            GVC.Instance.tilemap.walls.SetTile(position, tile);
        }

        private void SpawnHorizontalCorridor(Vector3Int currentCorridorPosition)
        {
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 2, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 1, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 1, 0));
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 2, 0));
        }

        private void SpawnVerticalCorridor(Vector3Int currentCorridorPosition)
        {
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x - 2, currentCorridorPosition.y, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x - 1, currentCorridorPosition.y, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y, 0));
            SpawnCorridorTile(new Vector3Int(currentCorridorPosition.x + 1, currentCorridorPosition.y, 0));
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x + 2, currentCorridorPosition.y, 0));
        }

        private void SpawnVerticalWalls(Vector3Int currentCorridorPosition)
        {
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 2, 0));
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y - 1, 0));
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y, 0));
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 1, 0));
            SpawnWallVerticalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y + 2, 0));
        }
        private void SpawnHorizontalWalls(Vector3Int currentCorridorPosition)
        {
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x - 2, currentCorridorPosition.y, 0));
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x - 1, currentCorridorPosition.y, 0));
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x, currentCorridorPosition.y, 0));
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x + 1, currentCorridorPosition.y, 0));
            SpawnWallHorizontalTile(new Vector3Int(currentCorridorPosition.x + 2, currentCorridorPosition.y, 0));
        }
        #endregion

        // Choose 2 doors from 2 rooms. Start with the first position and check if the position are horizontal or vertical
        // In case of an horizontal corridor:
        // Traverse the first half of the horizontal difference between the 2 room positions, after that spawn the vertical corridor.
        // Spawn the last half of the horizontal difference.
        internal void DrawCorridor(Door door, Vector3Int connectedDoorPosition)
        {
            Vector3Int difference = -door.position + connectedDoorPosition;
            Vector3Int differenceAbs = new Vector3Int(Math.Abs(difference.x), Math.Abs(difference.y), 0);
            Vector3Int currentCorridorPosition = door.position;

            int signX = 0;
            if (difference.x != 0 && differenceAbs.x != 0)
                signX = difference.x / differenceAbs.x;

            int signY = 0;
            if (difference.y != 0 && differenceAbs.y != 0)
                signY = difference.y / differenceAbs.y;

            if (door.direction == Direction.Right || door.direction == Direction.Left)
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

                if (corridorVerticalLength != 0)
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

            if (door.direction == Direction.Up || door.direction == Direction.Down)
            {
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
                    SpawnHorizontalCorridor(new Vector3Int(currentCorridorPosition.x + signX, currentCorridorPosition.y, 0)); // The plus signX since we don't want it to spawn in the already spawned corridor.
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
        }
    }
}
