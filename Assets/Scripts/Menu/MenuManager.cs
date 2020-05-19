using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //[SerializeField]Inpu
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadScene()
    {
        /*GetComponentInParent<Button>().interactable = false;
        CG.alpha = 1.0f;
        Logo.CrossFadeAlpha(0.0f, 0.0f, false);*/
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Player2");
        asyncLoad.allowSceneActivation = false;

        //Logo.CrossFadeAlpha(1.0f, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        while (asyncLoad.isDone)
            yield return null; //Make sure the map is fully loaded

        //Logo.CrossFadeAlpha(0.0f, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        asyncLoad.allowSceneActivation = true;
    }
}
