using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadMeScript : MonoBehaviour
{
    private float changeScenePauseTime = 1.0f;

    public void OnContinueSelected()
    {
        StartCoroutine(LoadGameScene("Level01"));
    }

    IEnumerator LoadGameScene(string sceneToLoad)
    {
        Debug.Log("Currently loading level: " + sceneToLoad);
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
