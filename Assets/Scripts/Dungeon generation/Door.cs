using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rooms
{
    [System.Serializable]
    public class Door
    {
        internal bool connected = false;

        public Vector3Int Position { get; set; }

        [SerializeField]
        public Direction Direction { get; set; }

        public Door(Vector3Int _dir)
        {
            this.Position = _dir;
        }

        public Door(Vector3Int position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public Direction GetOppositeDirection()
        {
            if (Direction == Direction.Down)
                return Direction.Up;

            if (Direction == Direction.Up)
                return Direction.Down;

            if (Direction == Direction.Left)
                return Direction.Right;

            if (Direction == Direction.Right)
                return Direction.Left;

            return Direction.Left;
        }
        public static Door operator +(Door door, Vector3Int position)
        {
            return new Door(door.Position + position, door.Direction);
        }


        public override string ToString()
        {
            return $"Direction: {Direction}, Position: {Position};";
        }

        internal void Unlock(Vector3Int roomPos)
        {
            Vector3Int dir = Vector3Int.zero; 
            switch(this.Direction)
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
            DungeonManager.tilemap_doors.SetTile(roomPos + Position,       DungeonManager.tile_Door_Unlocked);
            DungeonManager.tilemap_doors.SetTile(roomPos + Position + dir, DungeonManager.tile_Door_Unlocked);
            DungeonManager.tilemap_doors.SetTile(roomPos + Position - dir, DungeonManager.tile_Door_Unlocked);

            // Remove tiles on tilemap_walls
            DungeonManager.tilemap_walls.SetTile(roomPos + Position,       null);
            DungeonManager.tilemap_walls.SetTile(roomPos + Position + dir, null);
            DungeonManager.tilemap_walls.SetTile(roomPos + Position - dir, null);
        }

        internal void Lock(Vector3Int roomPos)
        {
            Vector3Int dir = Vector3Int.zero;
            switch (this.Direction)
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
            DungeonManager.tilemap_doors.SetTile(roomPos + Position, null);
            DungeonManager.tilemap_doors.SetTile(roomPos + Position + dir, null);
            DungeonManager.tilemap_doors.SetTile(roomPos + Position - dir, null);

            // Add tiles on tilemap_walls
            DungeonManager.tilemap_walls.SetTile(roomPos + Position, DungeonManager.tile_Door_Locked);
            DungeonManager.tilemap_walls.SetTile(roomPos + Position + dir, DungeonManager.tile_Door_Locked);
            DungeonManager.tilemap_walls.SetTile(roomPos + Position - dir, DungeonManager.tile_Door_Locked);
        }
    }
}
