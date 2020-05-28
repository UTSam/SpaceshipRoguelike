using UnityEngine;

public class KamiKaze : BasicMovingEnemy
{
    [SerializeField] private float kamikazeeDamage = 50;

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
        DeathAnimation();
        Debug.Log("animation death");
    }

    private void DeathAnimation()
    {
        if (GetComponent<Animate>())
        {
            GetComponent<Animate>().DoAnimationSpecial();
        }
    }
}
