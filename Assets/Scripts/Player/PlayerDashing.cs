/*
    Authors:
      Robbert Ritsema
*/

using System.Collections;
using UnityEngine;

public class PlayerDashing : State
{
    private readonly float dashTime = .06f;
    private readonly float dashSpeed = 50f;
    private readonly float maxDistance = 600f;

    private Vector2 input;

    private Rigidbody2D rb;
    private Vector3 originalPosition;

    private ParticleSystem animation;

    public PlayerDashing(Player player) : base(player)
    {
        rb = player.GetComponent<Rigidbody2D>();

        GameObject animationGO = GameObject.Find("dashAnimation");
        if (animationGO)
            animation = animationGO.GetComponent<ParticleSystem>();
    }

    public override void OnStateEnter()
    {
        originalPosition = rb.position;

        this.input.x = Input.GetAxisRaw("Horizontal");
        this.input.y = Input.GetAxisRaw("Vertical");
        if (this.input.magnitude > 1)
            this.input = this.input.normalized;

        player.GetComponent<HealthComponent>().isInvincible = true;

        if (animation)
        {
            ParticleSystem.MainModule newMain = animation.main;
            newMain.startRotation = -(player.transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            animation.Play();
        }
    }

    public override void OnStateExit()
    {
        player.GetComponent<HealthComponent>().isInvincible = false;
        if (animation) player.StartCoroutine(StopDashingAnimation());
    }

    //Make it so that the animation goes on for a while more, so that it looks better.
    IEnumerator StopDashingAnimation()
    {
        animation.emissionRate = 20;
        yield return new WaitForSeconds(.1f);
        if (animation) animation.Stop();
        animation.emissionRate = 70;
    }

    public override void Tick()
    {
        if (Vector2.Distance(rb.position, originalPosition) > maxDistance)
        {
            player.SetState(new PlayerMovement(player));
        }
    }

    public override void FixedTick()
    {
        rb.velocity = input * dashSpeed;
        player.StartCoroutine(StopDashing());
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        player.SetState(new PlayerMovement(player));
    }
}
