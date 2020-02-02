using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    GameObject easyStructure;

    [SerializeField]
    GameObject mediumStructure;

    [SerializeField]
    GameObject hardStructure;

    GameObject chosenStructure;

    string difficultyKey = "difficulty";

    void Start()
    {
        if (easyStructure != null)
        {
            int difficulty = PlayerPrefs.GetInt(difficultyKey);
            chosenStructure = GetStructure(difficulty);

            chosenStructure.SetActive(true);
        }
    }

    GameObject GetStructure(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                return easyStructure;
            case 2:
                return mediumStructure;
            case 3:
                return hardStructure;
            default:
                int random = Random.Range(1, 4);
                return GetStructure(random);
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }
}
