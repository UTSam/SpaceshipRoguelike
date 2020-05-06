using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingEntity
{

    // Forces variables
    private Vector2 acceleration;
    public float accelerationFactor = 0.6f;

    private Vector2 forceBack;
    public float forceBackFactor = 0.03f;

    // Aim variables
    public enum aimTypeList { mouse, keyboard };
    public aimTypeList aimType;

    // Orientation variables
    public float orientationFactorX = 3f;
    public float orientationFactorY = 1.5f;

    // Dash & looping variables
    public float movementCouldownDefault = 3;
    private float movementCouldown;
    public float dashFactor = 50f;
    public bool specialMovementActivated = false;
    public float loopingDoubleTapCooldownDefault = 0.3f;
    private float loopingDoubleTapCooldown;
    public string inputType, prevInputType;
    public bool loopTrigerActivated = false;
    public float loopBoostFactor = 5.0f;
    private int loopEndCount = 100;
    private string loopingOrientation = "none";


    public override void Start()
    {
       base.Start();
       movementCouldown = movementCouldownDefault;
       loopingDoubleTapCooldown = loopingDoubleTapCooldownDefault;
    }
    void Update()
    {
        // model Rotation
        if (aimType == aimTypeList.keyboard)
        {
            KeyboardAim();
        }
        else
        {
            MouseAim();
        }


        // Acceleration computation
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        speed = new Vector3(horizontalInput * 10.0f, verticalInput * 10.0f); acceleration.Set(horizontalInput, verticalInput);
        acceleration *= accelerationFactor;

        // Fore back computation
        forceBack = -speed * forceBackFactor;

        // special movements computation
        DashFunction();
        OrientModel();
        LoopingFunction();

        // Velocity computation
        speed += acceleration;
        speed += forceBack;

        UpdatePosition();
    }

    private void OnDestroy()
    {
        
    }


    public void OrientModel()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.y = 0;
        transform.rotation = Quaternion.Euler(eulerRotation);

        // SpaceShip normal orientation
        Vector2 orientationVector = Vector2.zero;
        orientationVector.y = (speed.x - acceleration.x) * orientationFactorX;
        orientationVector.x = (speed.y - acceleration.y) * orientationFactorY;

        // Spaceship looping modification
        if (loopingOrientation != "none")
        {
            if (loopingOrientation == "left" || loopingOrientation == "right")
            {
                orientationVector.y *= loopBoostFactor;
            }
                
            if (loopEndCount != 0)
                loopEndCount--;
            else
            {
                loopEndCount = 20;
                loopingOrientation = "none";
            }
        }

        // Orientation update
        eulerRotation.x = orientationVector.x;
        eulerRotation.y = orientationVector.y;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }


    public void DashFunction()
    {
        if (!specialMovementActivated && Input.GetKeyDown(KeyCode.RightShift))
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
        {
            Vector2 targetPosition = Vector2.zero;

            if (Input.GetKey(KeyCode.Keypad5))
                targetPosition += Vector2.up;

            if (Input.GetKey(KeyCode.Keypad2))
                targetPosition += Vector2.down;

            if (Input.GetKey(KeyCode.Keypad1))
                targetPosition += Vector2.left;

            if (Input.GetKey(KeyCode.Keypad3))
                targetPosition += Vector2.right;

            if (targetPosition != Vector2.zero)
            {
                Vector3 eulerRotation = transform.rotation.eulerAngles;
                eulerRotation.z = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.Euler(eulerRotation);
            }


        }
    }

    public void LoopingFunction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            inputType = "up";
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            inputType = "down";
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            inputType = "right";
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            inputType = "left";
        else
            inputType = "none";


        if(inputType != "none") // save the current input and initiate loop clock (1)
        {
            loopTrigerActivated = true;
            
            if(prevInputType != "none" && inputType != prevInputType) // If inputs are different reset the clock (2)
            {
                loopingDoubleTapCooldown = loopingDoubleTapCooldownDefault;
                prevInputType = "none";
            }

            if (inputType == prevInputType && !specialMovementActivated) // If inputs are the same -> starts the looping (2)
            {
                specialMovementActivated = true;
                loopingOrientation = inputType;
            }

            prevInputType = inputType;
        }


        if(loopingDoubleTapCooldown > 0 && loopTrigerActivated) // Decrease cooldown (-)
        {
            loopingDoubleTapCooldown -= Time.deltaTime;
        }


        if (loopingDoubleTapCooldown < 0) // reset loop clock (3)
        {
            loopingDoubleTapCooldown = loopingDoubleTapCooldownDefault;
            loopTrigerActivated = false;
            prevInputType = "none";
        }        
    }
}
