using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEngine;

public class CountDownDisplay : MonoBehaviour
{
    public float initalTime;
    public Text text;
    public Image timerImage;

    private float currentTime;
    string initialString;
    bool active = false;
    public float delayTime = 1;

    bool gameOver = false;

    public void TurnOn()
    {
        StartCoroutine(Delay());
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTime = initalTime;
        Debug.Assert(text != null);
        initialString = text.text;
    }

    private void Update()
    {
        if (active)
        {
            text.enabled = true;
            timerImage.enabled = true;
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                gameOver = true;
            }
            timerImage.fillAmount = currentTime / initalTime;
            text.text = initialString + currentTime.ToString("0");
        }
        else
        {
            timerImage.enabled = false;
            text.enabled = false;
        }

        if (gameOver)
        {
            active = false;
            GameplayManager gameManager = GetComponentInParent<GameplayManager>();
            gameManager.GameOver();
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        active = true;
    }
}
