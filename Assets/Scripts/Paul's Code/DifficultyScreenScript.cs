using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyScreenScript : MonoBehaviour
{
    private float changeScenePauseTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDifficulty1Selected()
    {
        SetDifficulty(1);
        StartCoroutine(LoadGameScene("ReadMeScene"));
    }
    public void OnDifficulty2Selected()
    {
        SetDifficulty(2);
        StartCoroutine(LoadGameScene("ReadMeScene"));
    }
    public void OnDifficulty3Selected()
    {
        SetDifficulty(3);
        StartCoroutine(LoadGameScene("ReadMeScene"));
    }


    void SetDifficulty(int difficultyLevel)
    {
        PlayerPrefs.SetInt("difficulty", difficultyLevel);
        PlayerPrefs.Save();
    }

    IEnumerator LoadGameScene(string sceneToLoad)
    {
        Debug.Log("Currently loading level: " + sceneToLoad);
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
