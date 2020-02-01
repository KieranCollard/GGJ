using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayLoopTestPaul : MonoBehaviour
{
    private float gameTime;
    // Start is called before the first frame update
    void Start()
    {
        gameTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("LeaderboardScene");
            LeaderboardScript.AddLeaderboardScoreEntry(gameTime, "noot(you)");
        }
    }
}
