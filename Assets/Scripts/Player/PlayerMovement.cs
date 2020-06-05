using System.Collections;
using UnityEngine;

public class PlayerMovement : State
{
    private float moveSpeed = 10;
    private Vector2 input;

    Rigidbody2D rb;

    public PlayerMovement(Player player) : base(player)
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    public override void Tick()
    {
        this.input.x = Input.GetAxisRaw("Horizontal");
        this.input.y = Input.GetAxisRaw("Vertical");
        if (this.input.magnitude > 1)
            this.input = this.input.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && this.input != Vector2.zero && player.canDash)
        {
            player.canDash = false;
            player.StartCoroutine(ResetDash());
            player.SetState(new PlayerDashing(player));
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(player.dashCooldown);
        player.canDash = true;
    }

    public override void FixedTick()
    {
        rb.velocity = input * moveSpeed;
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}