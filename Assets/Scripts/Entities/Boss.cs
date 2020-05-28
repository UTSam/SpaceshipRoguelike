using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public List<BossWeapon> weaponList = new List<BossWeapon>();
    public int Phase;
    protected List<BossWeapon> firingWeaponList = new List<BossWeapon>();

    public bool IsFiring = true;
    public float TimeBetweenShots = 1.0f;
    private float lastShotTimer = 0.0f;

    private BossWeapon lastShotWeapon;
    public bool IsReadyToFIre = true;

    [SerializeField] private GameObject ShieldPrefab;
    // Start is called before the first frame update
    void Start()
    {
        BossWeapon[] arr = GetComponentsInChildren<BossWeapon>();
        for (int i = 0; i < arr.Length; ++i)
        {
            weaponList.Add(arr[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShotTimer > TimeBetweenShots && IsFiring && IsReadyToFIre)
            FireRandomWeapons();

        lastShotTimer += Time.deltaTime;
    }
    private void FireRandomWeapons()
    {
        int index = 0;
        /*if (Phase < 2)
        {*/
            do { index = Random.Range(0, weaponList.Count); }
            while (weaponList[index] == lastShotWeapon);//never fire the same weapon twice in a row
            weaponList[index].InitFireSequence();

            lastShotWeapon = weaponList[index];
        /*}
        else
        {
            List<BossWeapon> tmpList = new List<BossWeapon>(weaponList);
            index = Random.Range(0, tmpList.Count);
            tmpList[index].InitFireSequence();
            tmpList.RemoveAt(index);
            index = Random.Range(0, tmpList.Count);
            tmpList[index].InitFireSequence();
        }*/
        lastShotTimer = 0.0f;
    }

    public void SetPhase(int nextPhase)
    {
        if (nextPhase > Phase)
        {
            Phase = nextPhase;
            StartCoroutine(InitNextPhaseRoutine());
        }
    }

    private IEnumerator InitNextPhaseRoutine()
     {
        lastShotWeapon.StopShooting();
        IsFiring = false;
        StartCoroutine(ShieldDropCoroutine(2));
        GetComponent<BossHealthComponent>().RestoreShield(int.MaxValue);
        yield return new WaitForSeconds(3);
        IsFiring = true;
    }

    private void LootShield()
    {
        GameObject shield = Instantiate(ShieldPrefab) as GameObject;
        shield.transform.position = weaponList[Random.Range(0, weaponList.Count)].transform.position;
    }

    private IEnumerator ShieldDropCoroutine(int nb)
    {
        for (int i=0; i<nb; i++)
        {
            LootShield();
            yield return new WaitForSeconds(1);
        }
    }
}
