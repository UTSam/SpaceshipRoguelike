using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    private BasicMovingEnemy host;

    static System.Random rand;
    private  Vector2 wanderTarget;
    private  Vector2 wanderCircle;
    private  Vector2 worldTarget;
    [SerializeField] private float wanderRadius = 1f;
    [SerializeField] private float wanderJitter = 0.25f;
    [SerializeField] private float wanderDistance = 0.4f * 0.4f;
    [SerializeField] private bool wanderOn = false;

    [SerializeField] private Deceleration arriveDeceleration = Deceleration.medium;
    [SerializeField] private bool seekON = false;
    [SerializeField] private bool arriveOn = false;
    [SerializeField] private bool persuitOn = false;

    [SerializeField] private bool fleeON = false;
    [SerializeField] private bool evadeOn = false;
    private double panicDistance = 2 * 2;


    private void Start()
    {
        host = GetComponent<BasicMovingEnemy>();
        wanderTarget = host.heading * wanderRadius;
        wanderCircle = new Vector2();
        worldTarget = new Vector2();
        rand = new System.Random();
    }

    public Vector2 Calculate()
    {

        Vector2 returnValue = Vector2.zero;

        if (seekON) returnValue += Seek();
        if (fleeON) returnValue += Flee();
        if (arriveOn) returnValue += Arrive(arriveDeceleration);
        if (persuitOn) returnValue += Persuit();
        if (evadeOn) returnValue += Evade()*4;
        if (wanderOn) returnValue += Wander();
        return returnValue;
    }

    public Vector2 ForwardComponent()
    {
        return Vector2.zero;
    }

    public Vector2 SideComponent()
    {
        return Vector2.zero;
    }

    public void SetPath() { }

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

    private Vector2 Persuit()
    {

        Vector2 toEvader = host.target.transform.position - host.transform.position;

        double relativeHeading = BasicEntity.DotProduct(host.heading, host.target.heading);

        if (BasicEntity.DotProduct(toEvader, host.heading) > 0 &&
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

        return Flee(host.GetPosition() + host.target.speed * lookAHeadTime);
    }

    private Vector2 Wander()
    {
        wanderCircle = host.GetPosition() + host.heading * wanderDistance;
        wanderTarget += new Vector2(
            rand.Next(-1, 1) * wanderJitter,
            rand.Next(-1, 1) * wanderJitter
            );
        wanderTarget = wanderTarget.normalized * wanderRadius;


        worldTarget = wanderCircle + wanderTarget;
        Vector2 force = (wanderTarget - host.GetPosition()).normalized;

        return force;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        if (host != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(host.GetPosition(), host.heading.normalized);
            if (wanderOn && wanderTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(host.GetPosition(), (wanderTarget - host.GetPosition()).normalized);
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(wanderTarget.normalized, 0.1f);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(worldTarget.normalized, 0.1f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(wanderCircle, 0.4f);
            }
        }
    }
}
enum Deceleration
{
    slow = 3,
    medium = 2,
    fast = 1
};

