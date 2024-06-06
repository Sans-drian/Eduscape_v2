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

    public UnityEvent setLocalTime;

    public static PointCounter instance;

    public TMP_Text pointText;
    public int currentKeys = 0;
    public int winCondition;

    public GameObject escapeButton;
    public GameObject escapeErrorText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pointText.text = currentKeys.ToString();
    }

    void Update()
    {
        if (currentKeys >= winCondition)
        {
            updateEscapeUI();
        }
    }

    public void increasePoints(int v)
    {
        currentKeys += v;
        pointText.text = currentKeys.ToString();
    }

    //set the color of the escape button and remove 'not enough points' text
    public void updateEscapeUI()
    {
        escapeButton.GetComponent<Image>().color = Color.white;
        escapeErrorText.SetActive(false);
    }

    //win button script; if the point amount is at the win condition, change scene to win.
    public void winButton()
    {
        if (currentKeys >= winCondition)
        {
            setLocalTime.Invoke();
            SceneManager.LoadScene("WinScreen");
        }
    }

}
