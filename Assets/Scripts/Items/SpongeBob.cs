using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpongeBob : BasicItem
{
    public override void OnPickUp(GameObject playerGO)
    {
        GlobalValues.Timer = GVC.Instance.StopWatchGO.GetComponentInChildren<Text>().text;
        SceneManager.LoadScene("VictoryScreen");
    }
}
