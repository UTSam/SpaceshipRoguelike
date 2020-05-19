using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] TMP_InputField inputField;
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadScene()
    {
        if (inputField.text != "")
            GlobalValues.Seed = int.Parse(inputField.text);

        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;


        while (asyncLoad.isDone)
            yield return null; //Make sure the map is fully loaded

        yield return new WaitForSeconds(1.0f); //1 second wait to make sure the map is fully loaded
        asyncLoad.allowSceneActivation = true;
    }
}
