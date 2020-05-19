using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MovingEntity
{
    public int Life { get; set; }

    // Forces variables
    public Vector2 acceleration;
    public float accelerationFactor = 0.6f;

    public Vector2 velocity;

    private Vector2 forceBack;
    public float forceBackFactor = 0.1f;

    // Aim variables
    public enum AimTypeList { mouse, keyboard };
    public AimTypeList aimType;

    // Orientation variables
    public float orientationFactorX = 5f;
    public float orientationFactorY = 3f;

    // Dash variables
    public float movementCouldownDefault = 1; // In seconds
    public float movementCouldown;
    public float dashFactor = 80;
    private bool specialMovementActivated = false;


    public override void Start()
    {
        base.Start();

        accelerationFactor = 0.6f;
        forceBackFactor = 0.1f;

        orientationFactorX = 5f;
        orientationFactorY = 3f;

        dashFactor = 80;
        movementCouldownDefault = 2; // In seconds
        movementCouldown = movementCouldownDefault;
        specialMovementActivated = false;
    }

    public void Update()
    {
        // model Rotation
        if (aimType == AimTypeList.keyboard)
        {
            KeyboardAim();
        } else
        {
            MouseAim();
        }

        // Acceleration computation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        acceleration.Set(horizontalInput, verticalInput);
        acceleration *= accelerationFactor;

        // Fore back computation
        forceBack = -velocity * forceBackFactor;

        // special movements computation
        OrientModel();
        DashFunction();

        // Velocity computation
        velocity += acceleration;
        velocity += forceBack;       

        Move(velocity); 
    }


    public void Damage(int damageAmount)
    {
        throw new System.NotImplementedException();
    }


    public void MouseAim()
    {
        Vector2 objectPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition -= objectPosition;

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.z = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }


    public void KeyboardAim()
    {
        Vector2 targetPosition = Vector2.zero;

        if (Input.GetKey(KeyCode.Keypad5))
            targetPosition += Vector2.up;

        if(Input.GetKey(KeyCode.Keypad2))
            targetPosition += Vector2.down;

        if (Input.GetKey(KeyCode.Keypad1))
            targetPosition += Vector2.left;

        if (Input.GetKey(KeyCode.Keypad3))
            targetPosition += Vector2.right;

        if(targetPosition != Vector2.zero)
        {
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.z = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(eulerRotation);

            
        }
        
    }

    public void OrientModel()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.y = 0;
        transform.rotation = Quaternion.Euler(eulerRotation);

        // SpaceShip normal orientation
        Vector2 orientationVector = Vector2.zero;
        orientationVector.y = (velocity.x - acceleration.x) * orientationFactorX;
        orientationVector.x = (velocity.y - acceleration.y) * orientationFactorY;


        // Orientation update
        eulerRotation.x = orientationVector.x;
        eulerRotation.y = orientationVector.y;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }


    public void DashFunction()
    {
        if (!specialMovementActivated && (Input.GetKeyDown(KeyCode.LeftShift) || (Input.GetKeyDown(KeyCode.RightShift))))
        {
            specialMovementActivated = true;

            acceleration *= dashFactor;
            forceBack *= dashFactor / 2;
        }

        if (specialMovementActivated)
            movementCouldown -= Time.deltaTime;

        if(movementCouldown < 0) // Reset couldown
        {
            movementCouldown = movementCouldownDefault;
            specialMovementActivated = false;
        }
    }

    public void Move(Vector2 displacementValue)
    {
        rigidBody.velocity = displacementValue;
        
        // Moves the weapons
        //GetComponent<WeaponsHandler>().MoveWeapons(displacementValue);
    }

}
