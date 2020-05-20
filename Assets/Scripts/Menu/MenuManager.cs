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
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadScene()
    {
        if (inputField.text != "")
            GlobalValues.Seed = int.Parse(inputField.text);
        else
            GlobalValues.Seed = Random.Range(int.MinValue, int.MaxValue);

        inputField.enabled = false;
        playButton.enabled = false;
        quitButton.enabled = false;
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
