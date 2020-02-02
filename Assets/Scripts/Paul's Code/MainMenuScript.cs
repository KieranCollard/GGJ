using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public string difficultyScene = "DifficultyScene";
    public string leaderboardScene = "LeaderboardScene";
    //public string ControlsScene = "Controls";

    private float changeScenePauseTime = 1.0f;

    void Start()
    {
    }

    public void OnPlayGameClicked()
    {
        StartCoroutine(LoadGameScene(difficultyScene));
    }

    public void OnLeaderBoardsClicked()
    {
        StartCoroutine(LoadGameScene(leaderboardScene));
    }

    public void OnCreditsClicked()
    {
        StartCoroutine(LoadGameScene(4));
    }

    public void OnExitGameClicked()
    {
        Application.Quit();
    }


    IEnumerator LoadGameScene(int sceneToLoad)
    {

        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator LoadGameScene(string sceneToLoad)
    {
        Debug.Log("Currently loading level: " + sceneToLoad);
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
