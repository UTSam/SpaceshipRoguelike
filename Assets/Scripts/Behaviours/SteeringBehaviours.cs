using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SteeringBehaviours : MonoBehaviour
{
    private BasicMovingEnemy host;
    private Vector2 returnValue;

    static System.Random rand;
    private Vector2 wanderTarget = Vector2.zero;
    private Vector2 wanderCircle = Vector2.zero;
    private Vector2 worldTarget = Vector2.zero;
    [SerializeField] private float wanderRadius = 1f;
    [SerializeField] private float wanderJitter = 0.25f;
    [SerializeField] private float wanderDistance = 0.4f * 0.4f;
    [SerializeField] private bool wanderOn = false;

    [SerializeField] private Deceleration arriveDeceleration = Deceleration.medium;
    [SerializeField] private bool seekON = false;
    [SerializeField] private bool arriveOn = false;
    [SerializeField] private bool pursuitOn = false;
    [SerializeField] private bool OffsetPursuitOn = false;
    [SerializeField] private bool wallAvoidanceON = false;
    [SerializeField] private BasicMovingEnemy Leader = null;
    [SerializeField] private Vector2 OffsetToleader = new Vector2(2, 2);

    private FeelerManager feelers;


    [SerializeField] private bool fleeON = false;
    [SerializeField] private bool evadeOn = false;

    [SerializeField] private float arriveForce = 1f;
    [SerializeField] private float fleeForce = 1f;
    private double panicDistance = 2 * 2;



    private void Start()
    {
        host = GetComponent<BasicMovingEnemy>();
        wanderTarget = host.heading * wanderRadius;
        rand = new System.Random();
        feelers = GetComponentInChildren<FeelerManager>();
    }

    public Vector2 Calculate()
    {
        returnValue = Vector2.zero;
        if (host.target != null)
        {
            if (wallAvoidanceON) returnValue += WallAvoidance();
            if (seekON) returnValue += Seek();
            if (fleeON) returnValue += Flee() * fleeForce;
            if (arriveOn) returnValue += Arrive(arriveDeceleration) * arriveForce;
            if (pursuitOn) returnValue += Pursuit();
            if (evadeOn) returnValue += Evade() * 2;
            if (wanderOn) returnValue += Wander();
            if (OffsetPursuitOn) returnValue += OffsetPursuit();
        }
        else
        {
            returnValue += Wander();
        }
        return returnValue;
    }

    private Vector2 Seek()
    {
        Vector2 desiredVelocity = (host.target.transform.position - host.transform.position) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Seek(Vector2 vector)
    {
        Vector2 desiredVelocity = (vector - host.GetPosition()) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }
    private Vector2 Flee()
    {
        if (host.CalculateDistance(host.target) > panicDistance)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (host.transform.position - host.target.transform.position) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Flee(Vector2 vector)
    {
        if (host.CalculateDistance(vector) > panicDistance)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (host.GetPosition() - vector) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Arrive(Deceleration deceleration)
    {
        Vector2 toTarget = host.target.transform.position - host.transform.position;
        float distance = host.CalculateDistance(toTarget);
        if (distance > 0.5)
        {
            const double decelerationTweaker = 0.3;
            double speed = distance / (((double)deceleration / 50) * decelerationTweaker);

            Vector2 desiredVelocity = toTarget * (float)speed / distance;
            return desiredVelocity;
        }
        return Vector2.zero;
    }

    private Vector2 Arrive(Vector2 target, Deceleration deceleration)
    {
        Debug.Log("In offset pursuit ARRIVE FUNCTION");
        Vector2 toTarget = target - host.GetPosition();
        float distance = host.CalculateDistance(toTarget);
        if (distance > 0.5)
        {
            const double decelerationTweaker = 0.3;
            double speed = distance / (((double)deceleration / 50) * decelerationTweaker);

            Vector2 desiredVelocity = toTarget * (float)speed / distance;
            return desiredVelocity;
        }
        return Vector2.zero;
    }

    private Vector2 Pursuit()
    {

        Vector2 toEvader = host.target.transform.position - host.transform.position;

        double relativeHeading = Vector2.Dot(host.heading, host.target.heading);

        if (Vector2.Dot(toEvader, host.heading) > 0 &&
            relativeHeading < -0.95) //-acos(0.95) = 18degrees
        {
            return Seek();
        }

        float lookAHeadTime =
            toEvader.magnitude /
            (host.maxSpeed + host.target.speed.magnitude);

        return Seek(host.target.GetPosition() + host.target.speed * lookAHeadTime);
    }

    private Vector2 Evade()
    {
        Vector2 toPersuer = host.target.transform.position - host.transform.position;

        float lookAHeadTime =
            toPersuer.magnitude /
            (host.maxSpeed + host.target.speed.magnitude);

        return Flee(host.target.GetPosition() + host.target.speed * lookAHeadTime);
    }

    private Vector2 Wander()
    {
        wanderCircle = host.heading * wanderDistance;
        wanderTarget += new Vector2(
            (float)(rand.NextDouble() * 2 - 1) * wanderJitter,
            (float)(rand.NextDouble() * 2 - 1) * wanderJitter
            );
        wanderTarget = wanderTarget.normalized * wanderRadius;
        worldTarget = wanderCircle + wanderTarget;

        return wanderTarget;
    }

    public Vector2 OffsetPursuit()
    {
        //No leader/target so no persuit
        if (this.Leader == null) return Vector2.zero;

        Vector2 WorldOffsetPos = Leader.GetPosition();
        //Calculate the heading of the leader and do this *-1 to get behind the leader.
        WorldOffsetPos += (Leader.heading.normalized * -1 * OffsetToleader);

        Vector2 toOffset = WorldOffsetPos - host.GetPosition();

        float lookAheadTime = toOffset.magnitude / (host.maxSpeed + Leader.maxSpeed);

        return Arrive(WorldOffsetPos + Leader.speed * lookAheadTime, Deceleration.VVVslow);
    }

    public Vector2 WallAvoidance()
    {
        if (feelers != null)
        {
            return feelers.CalculateForce();
        }
        else
            return Vector2.zero;
    }

    //***************************************************************************************************
    //DEBUG
    //***************************************************************************************************
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        if (host != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(host.GetPosition(), host.GetPosition() + host.heading);
            if (wanderOn && wanderTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(host.GetPosition(), host.GetPosition() + wanderTarget);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere((host.GetPosition() + wanderTarget), 0.1f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere((host.GetPosition() + worldTarget), 0.1f);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(host.GetPosition() + wanderCircle, 0.4f);
            }
        }
    }
}
enum Deceleration
{
    VVVslow = 10,
    VVslow = 5,
    Vslow = 4,
    slow = 3,
    medium = 2,
    fast = 1
};

