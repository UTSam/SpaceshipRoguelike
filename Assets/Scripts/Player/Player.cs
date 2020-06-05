using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MovingEntity
{
    private State currentState;
    private Rigidbody2D rb;

    public float dashCooldown;
    public bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetState(new PlayerMovement(this));
    }

    private void Update()
    {
        //OrientModel();
        LookAtMouse();

        currentState.Tick();
    }

    private void FixedUpdate()
    {
        currentState.FixedTick();
    }

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    private void LookAtMouse()
    {
        Vector2 objectPosition = rb.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition -= objectPosition;

        Vector3 eulerRotation = rb.transform.rotation.eulerAngles;
        eulerRotation.z = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg - 90;
        rb.transform.rotation = Quaternion.Euler(eulerRotation);
    }

    internal void GotHit(float damageValue, ElementType element)
    {
        HealthComponent hc = GetComponentInParent<HealthComponent>();
        if (!hc.isInvincible)
        {
            GetComponentInParent<HealthComponent>().Damage(damageValue, element);
            CameraShake.Shake();
        }
    }

    public  void OrientModel()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.y = 0;
        transform.rotation = Quaternion.Euler(eulerRotation);

        // SpaceShip normal orientation
        Vector2 orientationVector = Vector2.zero;
        orientationVector.y = rb.velocity.y * 4;
        orientationVector.x = rb.velocity.y * 4;


        // Orientation update
        eulerRotation.x = orientationVector.x;
        eulerRotation.y = orientationVector.y;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

}
