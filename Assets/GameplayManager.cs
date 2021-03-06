﻿using System.Collections;
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

    public TestDetect domtect;

    string difficultyKey = "difficulty";

    void Start()
    {
        if (easyStructure != null)
        {
            int difficulty = PlayerPrefs.GetInt(difficultyKey);
            chosenStructure = GetStructure(difficulty);

            chosenStructure.SetActive(true);
            domtect.SetDetectShape(difficulty - 1);
        }
    }

    private void Update()
    {
        if (domtect.finished)
        {
            GameOver();
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

        CountDownDisplay timer = GetComponentInChildren<CountDownDisplay>();
        float currentTime = timer.currentTime;
        float maxTime = timer.initalTime;
        float score = maxTime - currentTime;
        string name = "You";
        LeaderboardScript.AddLeaderboardScoreEntry(score, name);
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void StartGame()
    {
        domtect.turnOn = true;
    }

}
