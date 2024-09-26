using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public bool timerIsRunning = false;
    public Text timeText;
    string textSeconds;
    string textMinutes;
    void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out! You won!");
                timerIsRunning = false;
                timeRemaining = 0;
                SceneManager.LoadScene(3);
                Time.timeScale = 1;

            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textSeconds = seconds.ToString();
        textMinutes = minutes.ToString();

        timeText.text = (textMinutes + ":" + textSeconds );
        Debug.Log(timeText.text);
    }
}
