using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public string DifficultyScene = "DifficultyScene";
    public string ControlsScene = "Controls";

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
        StartCoroutine(LoadGameScene(1));
    }

    public void OnLeaderBoardsClicked()
    {
        StartCoroutine(LoadGameScene(2));
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

}
