using UnityEngine;

public class BasicMovingEnemy : MovingEntity
{
    private SteeringBehaviours steering;
    public Player target;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        steering = GetComponent<SteeringBehaviours>();
        this.mass = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate force of steering behaviours
        Vector2 steeringForce = this.steering.Calculate();

        //Acceleration of entity ==> Force/Mass
        Vector2 acceleration = steeringForce / this.mass;

        //Update velocity
        this.speed = acceleration * Time.deltaTime;
        speed *= 8;

        if (speed.sqrMagnitude > 0.0000001)
        {
            this.heading = speed.normalized;
            this.perpendicular = Perpendicular(heading);
        }
        UpdatePosition();
    }
}
