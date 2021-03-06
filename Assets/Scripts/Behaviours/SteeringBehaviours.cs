﻿/*
    Authors:
      Jelle van Urk
      Thibaut Rousselet
*/
using UnityEngine;
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
    [SerializeField] private float wanderForce = 1f;


    [SerializeField] private Deceleration arriveDeceleration = Deceleration.medium;
    [SerializeField] private bool seekON = false;
    [SerializeField] private float seekForce = 1f;

    [SerializeField] private bool arriveOn = false;
    [SerializeField] private float arriveForce = 1f;

    [SerializeField] private bool pursuitOn = false;
    [SerializeField] private float pursuitForce = 1f;

    [SerializeField] private bool OffsetPursuitOn = false;
    [SerializeField] private float offsetPursuitForce = 1f;
    [SerializeField] private Vector2 OffsetToleader = new Vector2(2, 2);
    [SerializeField] private BasicMovingEnemy Leader = null;

    [SerializeField] private bool fleeON = false;
    [SerializeField] private float fleeForce = 1f;

    [SerializeField] private bool evadeOn = false;
    [SerializeField] private float evadeForce = 1f;
    [SerializeField] private double panicDistance = 2 * 2;

    [SerializeField] private bool wallAvoidanceON = true;
    [SerializeField] private float wallForce = 5f;

    private bool kamikazeON = false;
    private Vector2 kamikazeDirection = Vector2.zero;
    [SerializeField] private float kamikazeForce = 1f;


    private FeelerManager feelers;


    private void Start()
    {
        host = GetComponent<BasicMovingEnemy>();
        wanderTarget = host.heading * wanderRadius;
        rand = new System.Random();
        feelers = GetComponentInChildren<FeelerManager>();
        if (feelers == null)
        {
            Debug.LogError("Feeler manager prefab missing");
        }
    }

    public virtual Vector2 Calculate()
    {
        returnValue = Vector2.zero;
        if (host.target != null && Vector3.Distance(host.transform.position, host.target.transform.position) < 20f)
        {
            if (kamikazeON)
            {
                if (kamikazeDirection == Vector2.zero)
                {
                    kamikazeDirection = (Kamikazeee() * kamikazeForce);
                    return kamikazeDirection;
                }
                else return kamikazeDirection;
            }
            else
            {
                if (wallAvoidanceON) returnValue += WallAvoidance() * wallForce;
                if (seekON) returnValue += Seek() * seekForce;
                if (fleeON) returnValue += Flee() * fleeForce;
                if (arriveOn) returnValue += Arrive(arriveDeceleration) * arriveForce;
                if (pursuitOn) returnValue += Pursuit() * pursuitForce;
                if (evadeOn) returnValue += Evade() * evadeForce;
                if (wanderOn) returnValue += Wander() * wanderForce;
                if (OffsetPursuitOn) returnValue += OffsetPursuit() * offsetPursuitForce;
            }
        }
        else
        {
            returnValue += Wander() * wanderForce;
            returnValue += WallAvoidance() * wallForce;
        }
        return returnValue;
    }

    private Vector2 Seek()
    {
        Vector2 desiredVelocity = (host.target.transform.position - host.transform.position).normalized * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Seek(Vector2 vector)
    {
        Vector2 desiredVelocity = (vector - host.GetPosition()).normalized * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Kamikazeee()
    {
        Vector2 desiredVelocity = (host.target.GetPosition() - host.GetPosition()).normalized * host.maxSpeed;
        return (desiredVelocity - host.speed);
    }

    private Vector2 Flee()
    {
        if (host.CalculateDistance(host.target) > panicDistance)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (host.GetPosition() - host.target.GetPosition()).normalized * host.target.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Flee(Vector2 vector)
    {
        if (host.CalculateDistance(vector) > panicDistance)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (host.GetPosition() - vector).normalized * host.target.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Arrive(Deceleration deceleration)
    {
        Vector2 toTarget = (host.target.transform.position - host.transform.position).normalized;
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
        Vector2 toTarget = (target - host.GetPosition()).normalized;
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

        Vector2 toEvader = (host.target.transform.position - host.transform.position).normalized;

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
        Vector2 toPersuer = (host.target.transform.position - host.transform.position).normalized;

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
        return feelers.CalculateForce();
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

    public void ENDISableKam()
    {
        this.kamikazeON = !this.kamikazeON;
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

