using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Serializable]
    public class StringEvent : UnityEvent<string> {} //create custom unity event class which can take in a string argument
    public StringEvent setElapsedTime;

    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI pauseMenuTimerText;

    [SerializeField]
    float remainingTime;
    float totalCountdownTime;

    float elapsedTime;
    private string elapsedTimeText;

    public float timeDecrease;
    
    void Start()
    {
        totalCountdownTime = remainingTime; //the total countdown time will be used to calculate the elapsed time
    }

    // Update is called once per frame
    void Update()
    {
        CountdownMethod();
        changeTimerColor();

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        pauseMenuTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CountdownMethod()
    {
        if (remainingTime > 0) //if timer is above 0
        {
            remainingTime -= Time.deltaTime;
        }
        else //if timer reaches 0
        {
            loseCondition();
            Debug.Log("Time's UP!");
            remainingTime = 0;
        }
    }

    private void changeTimerColor()
    {
        if (remainingTime < 60) //if timer is less than 1 minute
        {
            timerText.color = Color.red;
            pauseMenuTimerText.color = Color.red;
        }
    }

    public float getTimeDecrease()
    {
        return timeDecrease;
    }

    public void decreaseTime() //function to decrease the time by a certain amount (for when player presses incorrect answer)
    {
        remainingTime -= timeDecrease; 
    }

    public void loseCondition()
    {
        SceneManager.LoadScene("LoseScreen");
    }

    public void calculateTime()
    {
        elapsedTime = totalCountdownTime - remainingTime; //calculate elapsed time

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        elapsedTimeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        Debug.Log($"Elapsed time is: {elapsedTimeText}"); //debugging purposes

        setElapsedTime.Invoke(elapsedTimeText); //invoke event that holds method to set the elapsed time text
    }

}
