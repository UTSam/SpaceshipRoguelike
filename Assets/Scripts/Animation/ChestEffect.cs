using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Emission());
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator Emission()
    {
        yield return new WaitForSeconds(1);
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
