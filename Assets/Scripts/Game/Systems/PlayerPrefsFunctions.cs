using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsFunctions : MonoBehaviour
{
    private string playerName;
    private string qListName;
    private float avgCalcAns;
    private float elapsedTime;
    private string localTime;

    public void SaveDataInfo() //called in the main menu
    {
        PlayerPrefs.SetString("PlayerName", playerName); //set player prefs
        PlayerPrefs.SetString("QListName", qListName);


        //Code below for debugging purposes
        string inputName = PlayerPrefs.GetString("PlayerName");
        string inputQList = PlayerPrefs.GetString("QListName");

        Debug.Log($"PlayerName Playerprefs set as = {inputName}, QListName Playerprefs set as = {inputQList}");
    }

    public void SaveDataResults() //called after the game finishes
    {
        PlayerPrefs.SetFloat("AvgCalcAns", avgCalcAns); //set player prefs
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.SetString("LocalTime", localTime);


        //Code below for debugging purposes
        string inputAvgCalcAns = PlayerPrefs.GetString("AvgCalcAns");
        string inputElapsedTime = PlayerPrefs.GetString("ElapsedTime");
        string inputLocalTime = PlayerPrefs.GetString("LocalTime");

        Debug.Log($"AvgCalcAns Playerprefs set as = {inputAvgCalcAns}, ElapsedTime Playerprefs set as = {inputElapsedTime}, LocalTime Playerprefs set as {inputLocalTime}.");
    }


    // PUBLIC SET METHODS FOR THE PRIVATE VARIABLES BELOW
    public void setPlayerName(string input)
    {
        playerName = input;
    }

    public void setQListName(string input)
    {
        qListName = input;
    }

    public void setAvgCalcAns(float input)
    {
        avgCalcAns = input;
    }

    public void setElapsedTime(float input)
    {
        elapsedTime = input;
    }

    public void setLocalTime(string input)
    {
        localTime = input;
    }
}
