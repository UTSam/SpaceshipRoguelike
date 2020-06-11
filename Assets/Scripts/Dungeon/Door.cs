/*
    Authors:
      Robbert Ritsema
      Jacco Douma
*/

using Assets.Scripts.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Dungeon
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
        private bool locked = false;

        [SerializeField]
        public bool connected = false;

        [SerializeField]
        public Vector3Int position;

        [SerializeField]
        public Direction direction;

        public Corridor corridor = null;

        public Room room;

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
            this.locked = false;
            this.Draw(roomPos);
        }

        internal void Lock(Vector3Int roomPos)
        {
            this.locked = true;
            this.Draw(roomPos);
        }

        private Vector3Int RotateBasedOnDirection()
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

            return dir;
        }

        private void Draw(Vector3Int roomPos)
        {
            Tile wall = null;
            Tile door = null;

            if (locked)
            {
                wall = GVC.Instance.tiles.doorLocked;
            }
            else
            {
                door = GVC.Instance.tiles.doorUnlocked;
            }

            Vector3Int dir = RotateBasedOnDirection();

            // Add tiles on tilemap_doors
            GVC.Instance.tilemap.doors.SetTile(roomPos + position, door);
            GVC.Instance.tilemap.doors.SetTile(roomPos + position + dir, door);
            GVC.Instance.tilemap.doors.SetTile(roomPos + position - dir, door);

            // Remove tiles on tilemap_walls
            GVC.Instance.tilemap.walls.SetTile(roomPos + position, wall);
            GVC.Instance.tilemap.walls.SetTile(roomPos + position + dir, wall);
            GVC.Instance.tilemap.walls.SetTile(roomPos + position - dir, wall);
        }
    }
}
