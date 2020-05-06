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
    }
}
