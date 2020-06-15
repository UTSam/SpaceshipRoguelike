/*
    Authors:
      Jelle van Urk
*/
using UnityEngine;

public class Animate : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject bullitOnHitAnimation;
    [SerializeField] private GameObject specialAnimation;
    void Start()
    {
    }

    public void DoAnimationOnHit()
    {
        if (bullitOnHitAnimation != null) Instantiate(bullitOnHitAnimation, transform.position, Quaternion.identity);
    }

    public void DoAnimationSpecial()
    {
        if (specialAnimation != null) Instantiate(specialAnimation, transform.position, Quaternion.identity);
    }
}
