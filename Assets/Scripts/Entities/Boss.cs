/*
    Authors:
      Thibaut Rousselet
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public List<BossWeapon> weaponList = new List<BossWeapon>();

    public bool IsFiring = true;
    public float TimeBetweenShots = 1.0f;
    private float lastShotTimer = 0.0f;

    private BossWeapon lastShotWeapon;
    public bool IsReadyToFIre = true;
    private Vector3 startingPosition;

    [SerializeField] private CreditsText credits;
    [SerializeField] private GameObject DeathExplosionAnimation;
    [SerializeField] private GameObject FinalExplosionAnimation;
    private void FireRandomWeapons()
    {
        int index = 0;

        do { index = Random.Range(0, weaponList.Count); }
        while (weaponList[index] == lastShotWeapon);//never fire the same weapon twice in a row
        weaponList[index].InitFireSequence();

        lastShotWeapon = weaponList[index];

        lastShotTimer = 0.0f;
    }

    private IEnumerator Entrance()
    {
        GetComponent<BossHealthComponent>().isInvincible = true;
        while (startingPosition.y - transform.position.y < 3.0f)
        {
            transform.position += new Vector3(0f, -0.02f, 0f);
            yield return null;
        }

        Transform barTransform = GetComponentInChildren<LifeBar>().gameObject.transform;
        while (barTransform.localScale.x < 0.8f)
        {
            barTransform.localScale += new Vector3(0.02f, 0f, 0f);
            yield return null;
        }

        GetComponent<BossHealthComponent>().isInvincible = false;
        yield return new WaitForSeconds(0.5f);
        IsFiring = true;
        yield return null;
    }

    private IEnumerator LeaveScreen()
    {
        while (startingPosition.y - transform.position.y > 0.0f)
        {
            transform.position += new Vector3(0f, +0.01f, 0f);
            yield return null;
        }

        Instantiate(FinalExplosionAnimation, transform.position, Quaternion.identity);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
            Instantiate(FinalExplosionAnimation, new Vector3(spawnX, spawnY, 0) , Quaternion.identity);
        }
    }

    public IEnumerator PlayDeathAnimation(int nbExplosions)
    {
        GVC.Instance.StopWatchGO.GetComponent<StopWatchScript>().IsPaused = true;
        credits.StartCoroutine(credits.Scroll());
        StartCoroutine(LeaveScreen());
        IsFiring = false;
        for (int i = 0; i < nbExplosions; i++)
        {
            Instantiate(DeathExplosionAnimation, transform.position +
                new Vector3(Random.Range(-4f, 4f), Random.Range(-1f, -3f), 0f)
                , Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

    }
}