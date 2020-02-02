using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderboardScript : MonoBehaviour
{
    public Button returnButton;
    private float changeScenePauseTime = 1.0f;

    string mainMenuString = "MainMenuScene";

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;
        
    private void Awake()
    {
        
        entryContainer = transform.Find("LeaderboardEntryContainer");
        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");

        Button returnButton = gameObject.GetComponent<Button>();

        entryTemplate.gameObject.SetActive(false);

        AddLeaderboardScoreEntry(10000.0f, "Butts");

        string jsonString = PlayerPrefs.GetString("leaderboardTable");
        LeaderboardScores leaderboardScores = JsonUtility.FromJson<LeaderboardScores>(jsonString);


        SortScores(leaderboardScores.leaderboardEntryList);


        leaderboardEntryTransformList = new List<Transform>();
        foreach (LeaderboardEntry leaderboardEntry in leaderboardScores.leaderboardEntryList)
        {
            CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);
        }
    }

    private void CreateLeaderboardEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20.0f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int pos = transformList.Count + 1;

        string posString;
        switch (pos)
        {
            default:
                posString = pos + "TH"; break;
            case 1: posString = "1ST"; break;
            case 2: posString = "2ND"; break;
            case 3: posString = "3RD"; break;
        }

        entryTransform.Find("Pos").GetComponent<Text>().text = posString;

        float score = leaderboardEntry.score;
        entryTransform.Find("Score").GetComponent<Text>().text = score.ToString();

        string name = leaderboardEntry.name;
        entryTransform.Find("Name").GetComponent<Text>().text = name;

        entryTransform.Find("Background").gameObject.SetActive(pos % 2 == 1);

        if (pos == 1)
        {
            //highlight first
            entryTransform.Find("Pos").GetComponent<Text>().color = Color.green;
            entryTransform.Find("Score").GetComponent<Text>().color = Color.green;
            entryTransform.Find("Name").GetComponent<Text>().color = Color.green;
        }


        RectTransform returnButtonRectTransform = returnButton.GetComponent<RectTransform>();
        returnButtonRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count + 10);
        //returnButton.gameObject.transform as RectTransform;
        //= returnButtonRectTransform.anchoredPosition.y - 30;

        transformList.Add(entryTransform);
    }

    static public void AddLeaderboardScoreEntry(float score, string name)
    {
        //create leaderboardEntry
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry { score = score, name = name };

        //Load saved leaderboardScores
        string jsonString = PlayerPrefs.GetString("leaderboardTable");
        if (jsonString == "")
        {
            SetNootLeaderBoard();
            jsonString = PlayerPrefs.GetString("leaderboardTable");
            //JsonUtility.ToJson()
        }
        LeaderboardScores leaderboardScores = JsonUtility.FromJson<LeaderboardScores>(jsonString);



        //Add new entry to leaderboardScores
        leaderboardScores.leaderboardEntryList.Add(leaderboardEntry);

        while(leaderboardScores.leaderboardEntryList.Count > 15)
        {
            leaderboardScores.leaderboardEntryList.RemoveAt(15);
        }

        //save updated leaderboardScores
        string json = JsonUtility.ToJson(leaderboardScores);
        PlayerPrefs.SetString("leaderboardTable", json);
        PlayerPrefs.Save();
    }
    static public void SetNootLeaderBoard()
    {
        List<LeaderboardEntry> leaderboardEntryList = new List<LeaderboardEntry>()
        {
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
            new LeaderboardEntry{ score = 999, name = "Noot Noot"},
        };
        LeaderboardScores leaderboardScores = new LeaderboardScores {
            
        };
        leaderboardScores.leaderboardEntryList = leaderboardEntryList;

        //save updated leaderboardScores
        string json = JsonUtility.ToJson(leaderboardScores);
        PlayerPrefs.SetString("leaderboardTable", json);
        PlayerPrefs.Save();
    }

    private class LeaderboardScores
    {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    [System.Serializable]
    private class LeaderboardEntry
    {
        public float score;
        public string name;
    }

    private void SortScores(List<LeaderboardEntry> leaderboardEntryList)
    {
        for (int i = 0; i < leaderboardEntryList.Count; i++)
        {
            for (int j = 0; j < leaderboardEntryList.Count; j++)
            {
                if (leaderboardEntryList[i].score < leaderboardEntryList[j].score)
                {
                    LeaderboardEntry temp = leaderboardEntryList[j];
                    leaderboardEntryList[j] = leaderboardEntryList[i];
                    leaderboardEntryList[i] = temp;
                }
            }
        }
    }

    private void SetRetunPosition()
    {
        
    }
    public void OnMainMenuClicked()
    {
        StartCoroutine(LoadGameScene(mainMenuString));
    }

    IEnumerator LoadGameScene(int sceneToLoad)
    {

        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator LoadGameScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(changeScenePauseTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    

}
