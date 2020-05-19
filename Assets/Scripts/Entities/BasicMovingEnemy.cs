using UnityEngine;

[RequireComponent(typeof(SteeringBehaviours))]

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
        this.target = Main.Instance.PlayerGO.GetComponent<Player>();
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

        if (speed.sqrMagnitude > 0.0000001)
        {
            this.heading = speed.normalized;
            this.perpendicular = Vector2.Perpendicular(heading);
        }
        if (target != null)
        {
            Vector3 lookPos = target.transform.position - transform.position;
            float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        UpdatePosition();
    }
}
