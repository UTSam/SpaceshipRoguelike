/*
    Authors:
      Thibaut Rousselet
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsText : MonoBehaviour
{
    public RectTransform trans;

    public IEnumerator Scroll()
    {
        while (trans.anchoredPosition.y < Screen.height + 300)
        {
            trans.position += new Vector3(0f, 0.8f, 0f);
            yield return null;
        }
        GlobalValues.Timer = GVC.Instance.StopWatchGO.GetComponent<StopWatchScript>().textZone.text;
      Destroy(GVC.Instance.PlayerGO);
      SceneManager.LoadScene("VictoryScreen");
    }
}
