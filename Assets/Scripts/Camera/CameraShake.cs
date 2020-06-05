using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraScript))]
public class CameraShake : MonoBehaviour
{
    private CameraScript cameraScript;
    public static CameraShake _instance;

    [SerializeField] private float duration = 2f;
    [SerializeField] private float amount = 0.02f;

    void Awake()
    {
        _instance = this;
        cameraScript = GetComponent<CameraScript>();
    }

    public static void Shake()
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.cShake());
    }

    public IEnumerator cShake()
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            Vector3 shakeChange = cameraScript.GetPosition() + Random.insideUnitSphere * amount;
            shakeChange.z = transform.localPosition.z;

            transform.localPosition = shakeChange;

            yield return null;
        }
    }
}