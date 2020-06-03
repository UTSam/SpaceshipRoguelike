using System.Collections;
using UnityEngine;

public class KamiKaze : BasicMovingEnemy
{
    [SerializeField] private float kamikazeeDamage = 50;
    [SerializeField] private float howLongToWait = 0.3f;
    private bool ableToMove = false;
    private float LifeSpan = 4f;

    public override void Start()
    {
        base.Start();
        StartCoroutine(waitForMoving());
    }

    protected override void Update()
    {
        base.Update();
        LifeSpan -= Time.deltaTime;

        if (LifeSpan <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponentInParent<HealthComponent>().Damage(kamikazeeDamage);
            if (GetComponent<Animate>())
            {
                GetComponent<Animate>().DoAnimationOnHit();
            }
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        //DeathAnimation();
    }

    private void DeathAnimation()
    {
        if (GetComponent<Animate>())
        {
            GetComponent<Animate>().DoAnimationSpecial();
        }
    }

    IEnumerator waitForMoving()
    {
        if (ableToMove == false)
        {
            yield return new WaitForSeconds(howLongToWait);
            GetComponent<SteeringBehaviours>().ENDISableKam();
            ableToMove = true;
        }
    }
}
