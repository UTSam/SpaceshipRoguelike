using UnityEngine;

public class KamiKaze : BasicMovingEnemy
{
    [SerializeField] private float kamikazeeDamage = 50;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Debug.Log("in the damage statement");
            collision.gameObject.GetComponentInParent<HealthComponent>().Damage(kamikazeeDamage);
            Destroy(this.gameObject);
        }
        Debug.Log("outside damage statement");
    }
}
