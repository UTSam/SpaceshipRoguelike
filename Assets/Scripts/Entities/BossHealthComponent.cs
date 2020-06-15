/*
    Authors:
      Thibaut Rousselet
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealthComponent : HealthComponent
{
    private Boss boss;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        boss = GetComponent<Boss>();

        SceneManager.MoveGameObjectToScene(GVC.Instance.PlayerGO, SceneManager.GetActiveScene());
        GVC.Instance.PlayerGO.GetComponent<Boundaries>().MainCamera = FindObjectOfType<Camera>();
        GVC.Instance.PlayerGO.GetComponent<Boundaries>().Init();
    }

    public override void Damage(float damageValue)
    {
        base.Damage(damageValue);
    }

    public override void Damage(float damageValue, ElementType elem)
    {
        base.Damage(damageValue, elem);
    }

    public override void OnDeath()
    {
        boss.IsFiring = false;
        if (bar != null)
        {
            Destroy(bar.gameObject);
            boss.StartCoroutine(boss.PlayDeathAnimation(500));
        } 
    }
}
