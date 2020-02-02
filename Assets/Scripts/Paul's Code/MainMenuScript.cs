using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public string difficultyScene = "DifficultyScene";
    public string leaderboardScene = "LeaderboardScene";
    //public string ControlsScene = "Controls";

    public AudioClip buttonClickSound;
    public AudioSource source;
    private float volume = 1.0f;
    private float changeScenePauseTime = 1.0f;

    void Start()
    {
        source = GetComponent<AudioSource>();
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

        source.PlayOneShot(buttonClickSound, volume);
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator LoadGameScene(string sceneToLoad)
    {

        source.PlayOneShot(buttonClickSound, volume);
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
