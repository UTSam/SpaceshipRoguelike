using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingEntity
{
<<<<<<< Updated upstream
    public override void Start()
    {
        base.Start();
=======
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
    public float movementCouldown;
    public float dashFactor = 50f;
    public bool specialMovementActivated = false;
    public float loopingDoubleTapCooldownDefault = 0.3f;
    public float loopingDoubleTapCooldown;
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
>>>>>>> Stashed changes
    }
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
<<<<<<< Updated upstream
        Speed = new Vector2(horizontalInput*10.0f, verticalInput*10.0f);
=======
        acceleration.Set(horizontalInput, verticalInput);
        acceleration *= accelerationFactor;

        // Fore back computation
        forceBack = -Speed * forceBackFactor;

        // special movements computation
        DashFunction();
        OrientModel();
        LoopingFunction();

        // Velocity computation
        Speed += acceleration;
        Speed += forceBack;

>>>>>>> Stashed changes
        UpdatePosition();
    }

    private void OnDestroy()
    {
        
    }
<<<<<<< Updated upstream
=======


    public void OrientModel()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.x = 0;
        eulerRotation.y = 0;
        transform.rotation = Quaternion.Euler(eulerRotation);

        // SpaceShip normal orientation
        Vector2 orientationVector = Vector2.zero;
        orientationVector.y = (Speed.x - acceleration.x) * orientationFactorX;
        orientationVector.x = (Speed.y - acceleration.y) * orientationFactorY;

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
>>>>>>> Stashed changes
}
