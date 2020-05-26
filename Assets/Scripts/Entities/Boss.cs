using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public List<BossWeapon> weaponList = new List<BossWeapon>();
    protected List<BossWeapon> firingWeaponList = new List<BossWeapon>();

    private BossWeapon lastShotWeapon;
    // Start is called before the first frame update
    void Start()
    {
        BossWeapon[] arr = GetComponentsInChildren<BossWeapon>();
        for (int i = 0; i < arr.Length; ++i)
        {
            weaponList.Add(arr[i]);
        }
        InvokeRepeating("FireRandomWeapons", 0.0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FireRandomWeapons()
    {
        int index = 0;
        do { index = Random.Range(0, weaponList.Count); }
        while (weaponList[index] == lastShotWeapon);
        weaponList[index].InitShotSequence();
        lastShotWeapon = weaponList[index];
    }
}
