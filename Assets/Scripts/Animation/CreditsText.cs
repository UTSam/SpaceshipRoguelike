using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsText : MonoBehaviour
{
    public RectTransform trans;
   /* private void Start()
    {
        trans = GetComponent<RectTransform>();
    }*/

    public IEnumerator Scroll()
    {
        while (trans.localPosition.y < 600)
        {
            trans.position += new Vector3(0f, 0.5f, 0f);
            yield return null;
        }
    }
}
