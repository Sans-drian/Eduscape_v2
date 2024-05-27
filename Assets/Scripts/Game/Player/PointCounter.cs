using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    public static PointCounter instance;

    public TMP_Text pointText;
    public int currentPoints = 0;
    public int winCondition = 100;

    public GameObject escapeButton;
    public GameObject escapeErrorText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pointText.text = currentPoints.ToString();
    }

    void Update()
    {
        if (currentPoints >= winCondition)
        {
            updateEscapeUI();
        }
    }

    public void increasePoints(int v)
    {
        currentPoints += v;
        pointText.text = currentPoints.ToString();
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
        if (currentPoints >= winCondition)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

}
