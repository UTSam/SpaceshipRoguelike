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

    private ParticleSystem dashAnimation;

    public PlayerDashing(Player player) : base(player)
    {
        rb = player.GetComponent<Rigidbody2D>();

        GameObject animationGO = GameObject.Find("dashAnimation");
        if (animationGO)
            dashAnimation = animationGO.GetComponent<ParticleSystem>();
    }

    public override void OnStateEnter()
    {
        player.StartCoroutine(ResetDashTimer());
        player.StartCoroutine(StopDashing());

        originalPosition = rb.position;

        this.input.x = Input.GetAxisRaw("Horizontal");
        this.input.y = Input.GetAxisRaw("Vertical");
        if (this.input.magnitude > 1)
            this.input = this.input.normalized;

        player.GetComponent<HealthComponent>().isInvincible = true;

        if (dashAnimation)
        {
            ParticleSystem.MainModule newMain = dashAnimation.main;
            newMain.startRotation = -(player.transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
            dashAnimation.Play();
        }
    }

    IEnumerator ResetDashTimer()
    {
        //animation
        yield return new WaitForSeconds(player.dashCooldown);
        player.canDash = true;

        Color color = new Color(0, 164, 255);
        player.GetComponent<SpriteRenderer>().color = color;
        player.StartCoroutine(ResetColor());
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(.15f);
        //animation
        Color color = new Color(255, 255, 255);
        player.GetComponent<SpriteRenderer>().color = color;
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
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        player.SetState(new PlayerMovement(player));
    }

    public override void OnStateExit()
    {
        player.GetComponent<HealthComponent>().isInvincible = false;
        if (dashAnimation) player.StartCoroutine(StopDashingAnimation());
    }

    //Make it so that the animation goes on for a while more, so that it looks better.
    IEnumerator StopDashingAnimation()
    {
        dashAnimation.emissionRate = 20;
        yield return new WaitForSeconds(.1f);
        if (dashAnimation) dashAnimation.Stop();
        dashAnimation.emissionRate = 70;
    }
}
