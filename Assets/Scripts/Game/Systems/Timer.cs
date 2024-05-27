using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    float remainingTime;
    float totalCountdownTime;

    float elapsedTime;
    private string elapsedTimeText;

    [SerializeField]
    float timeDecrease;
    
    void Start()
    {
        totalCountdownTime = remainingTime; //the total countdown time will be used to calculate the elapsed time
    }

    // Update is called once per frame
    void Update()
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

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
    }

    public string GetElapsedTime() //get method for the elapsed time
    {
        return elapsedTimeText;
    }

    
}
