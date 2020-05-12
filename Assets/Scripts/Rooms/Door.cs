using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Rooms
{
    public class Door
    {
        internal bool connected = false;

        public Vector3Int Position { get; set; }

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

        public override string ToString()
        {
            return $"Direction: {Direction}, Position: {Position};";
        }
    }
}
