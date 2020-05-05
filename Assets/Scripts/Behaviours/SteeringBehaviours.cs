using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    private BasicMovingEnemy host;
    private Vector2 target;
    [SerializeField] private Deceleration arriveDeceleration;
    [SerializeField] private bool seekON;
    [SerializeField] private bool fleeON;
    [SerializeField] private bool arriveOn;
    [SerializeField] private bool persuitOn;

    private void Start()
    {
        host = GetComponent<BasicMovingEnemy>();
    }

    public Vector2 Calculate()
    {

        Vector2 returnValue = Vector2.zero;

        if (seekON) returnValue += Seek() * 4;
        if (fleeON) returnValue += Flee() * 4;
        if (arriveOn) returnValue += Arrive(arriveDeceleration);
        if (persuitOn) returnValue += Persuit()*4;
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
    public void SetTarget(Vector2 target)
    {
        this.target = target;
    }
    public void SetTargetEntity01(BasicEntity entity) { }
    public void SetTargetEntity02(BasicEntity entity) { }

    private Vector2 Seek()
    {
        Vector2 desiredVelocity = (host.target.transform.position - host.transform.position) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Seek(Vector2 vector)
    {
        Vector2 desiredVelocity = (vector - new Vector2(host.transform.position.x , host.transform.position.y)) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }
    private Vector2 Flee()
    {
        float panicDistance = 3 * 3;

        if (host.CalculateDistance(host.target) > panicDistance)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = (host.transform.position - host.target.transform.position) * host.maxSpeed;
        return desiredVelocity - host.speed;
    }

    private Vector2 Arrive(Deceleration deceleration)
    {
        Vector2 toTarget = host.target.transform.position - host.transform.position;
        float distance = host.CalculateDistance(toTarget);
        if ( distance > 0.5)
        {
            const double decelerationTweaker = 0.3;
            double speed = distance / (((double)deceleration/50) * decelerationTweaker);

            Vector2 desiredVelocity = toTarget * (float)speed / distance;
            return desiredVelocity;
        }
        return Vector2.zero;
    }

    private Vector2 Persuit()
    {

        Vector2 toEvader = host.target.transform.position - host.transform.position;

        double relativeHeading = BasicEntity.DotProduct(host.heading, host.target.heading);

        if(BasicEntity.DotProduct(toEvader, host.heading) > 0 &&
            relativeHeading < -0.95) //-acos(0.95) = 18degrees
        {
            return Seek();
        }

        float lookAHeadTime = toEvader.magnitude/(host.maxSpeed + host.target.speed.sqrMagnitude);

        return Seek(new Vector2(host.target.transform.position.x, host.target.transform.position.y) + host.target.speed * lookAHeadTime);
    }
}
enum Deceleration
{
    slow = 3,
    medium = 2,
    fast = 1
};