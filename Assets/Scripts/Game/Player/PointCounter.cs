using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class PointCounter : MonoBehaviour
{

    /*
    ===========================================================================

    NOTE FOR CLARIFICATION:
    The meaning of "Points" and "Keys" is interchangeable. The game at first was designed with points in mind, but the in game UI text shows 
    "Keys Collected" instead of "Points Collected" (and some other things in the inspector too) due to a change later on, but some other scripts 
    and things related to this script/game score counter remains as "points" and not "keys".

    ===========================================================================
    */

    public UnityEvent setAvgAnsAcc; //set average answer accuracy
    public UnityEvent setElapsedTime;
    public UnityEvent setLocalTime;
    public UnityEvent saveGameResults;

    public static PointCounter instance;

    public TMP_Text pointText;
    public int currentKeys = 0;

    [SerializeField]
    private int winCondition;

    public GameObject escapeButton;
    public GameObject escapeErrorText;
    [SerializeField]
    private QuestionSetup questionSetup;

    [SerializeField]
    private ExitDoor exitDoor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pointText.text = currentKeys.ToString() + $"/{questionSetup.questionsCount}";
        winCondition = questionSetup.questionsCount; //the win condition should be set the same amount as how many questions there are
        Debug.Log($"The win condition is set to {winCondition}");
    }

    void Update()
    {
        if (currentKeys >= winCondition)
        {
            updateEscapeUI();
            changeExitDoorSprite();
        }
    }

    //set the color of the escape button and remove 'not enough points' text
    public void updateEscapeUI()
    {
        escapeButton.GetComponent<Image>().color = Color.white;
        escapeErrorText.SetActive(false);
    }

    private void changeExitDoorSprite()
    {
        exitDoor.changeExitDoorSprite();
    }


    public void increasePoints(int v)
    {
        currentKeys += v;
        pointText.text = currentKeys.ToString() + $"/{questionSetup.questionsCount}";
    }


    //win button script; when pressed, if the point amount is at the win condition, change scene to win.
    public void winButton()
    {
        if (currentKeys >= winCondition)
        {
            //these invokes below are for saving the data into the playerprefs inside unity, not saving the results to the database.
            setAvgAnsAcc.Invoke();
            setElapsedTime.Invoke();
            setLocalTime.Invoke();
            saveGameResults.Invoke();
            
            SceneManager.LoadScene("WinScreen");
        }
    }

}
