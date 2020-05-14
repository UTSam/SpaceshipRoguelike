using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rooms
{
    [System.Serializable]
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    [System.Serializable]
    public class Door
    {
        [SerializeField]
        public bool connected = false;

        [SerializeField]
        public Vector3Int position;

        [SerializeField]
        public Direction direction;

        public Door(Vector3Int _dir)
        {
            this.position = _dir;
        }

        public Door(Vector3Int position, Direction direction)
        {
            this.position = position;
            this.direction = direction;
        }

        public Direction GetOppositeDirection()
        {
            if (direction == Direction.Down)
                return Direction.Up;

            if (direction == Direction.Up)
                return Direction.Down;

            if (direction == Direction.Left)
                return Direction.Right;

            if (direction == Direction.Right)
                return Direction.Left;

            return Direction.Left;
        }

        public static Door operator +(Door door, Vector3Int position)
        {
            return new Door(door.position + position, door.direction);
        }

        internal void Unlock(Vector3Int roomPos)
        {
            Vector3Int dir = Vector3Int.zero; 
            switch(this.direction)
            {
                case Direction.Up:
                case Direction.Down:
                    dir = Vector3Int.left;
                    break;
                case Direction.Left:
                case Direction.Right:
                    dir = Vector3Int.up;
                    break;
                default:
                    break;
            }

            // Add tiles on tilemap_doors
            DungeonManager.tilemap_doors.SetTile(roomPos + position,       DungeonManager.tile_Door_Unlocked);
            DungeonManager.tilemap_doors.SetTile(roomPos + position + dir, DungeonManager.tile_Door_Unlocked);
            DungeonManager.tilemap_doors.SetTile(roomPos + position - dir, DungeonManager.tile_Door_Unlocked);

            // Remove tiles on tilemap_walls
            DungeonManager.tilemap_walls.SetTile(roomPos + position,       null);
            DungeonManager.tilemap_walls.SetTile(roomPos + position + dir, null);
            DungeonManager.tilemap_walls.SetTile(roomPos + position - dir, null);
        }

        internal void Lock(Vector3Int roomPos)
        {
            Vector3Int dir = Vector3Int.zero;
            switch (this.direction)
            {
                case Direction.Up:
                case Direction.Down:
                    dir = Vector3Int.left;
                    break;
                case Direction.Left:
                case Direction.Right:
                    dir = Vector3Int.up;
                    break;
                default:
                    break;
            }

            // Remove tiles on tilemap_doors
            DungeonManager.tilemap_doors.SetTile(roomPos + position, null);
            DungeonManager.tilemap_doors.SetTile(roomPos + position + dir, null);
            DungeonManager.tilemap_doors.SetTile(roomPos + position - dir, null);

            // Add tiles on tilemap_walls
            DungeonManager.tilemap_walls.SetTile(roomPos + position, DungeonManager.tile_Door_Locked);
            DungeonManager.tilemap_walls.SetTile(roomPos + position + dir, DungeonManager.tile_Door_Locked);
            DungeonManager.tilemap_walls.SetTile(roomPos + position - dir, DungeonManager.tile_Door_Locked);
        }
    }
}
