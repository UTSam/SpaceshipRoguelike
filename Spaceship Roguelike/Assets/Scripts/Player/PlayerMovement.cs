using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    class PlayerMovement
    {
        public float moveSpeed = 5f;
        private Rigidbody2D rb;

        Vector2 movement;

        public PlayerMovement(Rigidbody2D rb)
        {
            this.rb = rb;
        }

        internal void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        internal void FixedUpdate()
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
