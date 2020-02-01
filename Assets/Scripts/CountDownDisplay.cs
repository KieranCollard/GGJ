using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

using UnityEngine;

public class CountDownDisplay : MonoBehaviour
{
    public float initalTime;
    public Text text;
    public float currentTime;
    string initialString;
    bool active = false;
    public float delayTime = 1;
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

    private void FixedUpdate()
    {
        if (active)
        {
            text.enabled = true; 
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = 0;
            }
            text.text = initialString + currentTime.ToString("0.00");
        }
        else
        {
            text.enabled = false;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        active = true;
    }
}
