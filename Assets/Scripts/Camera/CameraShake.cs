using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraScript))]
public class CameraShake : MonoBehaviour
{
    private CameraScript cameraScript;
    private Vector3 _originalPos;
    public static CameraShake _instance;

    void Awake()
    {
        _originalPos = transform.localPosition;
        _instance = this;
        cameraScript = GetComponent<CameraScript>();
    }

    public static void Shake(float duration, float amount)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            transform.localPosition = cameraScript.GetPosition() + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }

}