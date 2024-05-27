using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;

public class AnswerButton : MonoBehaviour
{
    private bool isCorrect; //know if this button is the correct answer
    private bool isPressed; //know if this button has been pressed or not
    [SerializeField]
    private TextMeshProUGUI answerText; //the answer text that will be changed

    [SerializeField]
    private PointCounter pointCounter;
    [SerializeField]
    private Timer timer;

    public ChestManager chestManager;

    private int wrongClickedCount;
    private int correctClickedCount;

    private int correctPointAmount = 1;

    public void SetAnswerText(string newText) //get text from other script
    {
        answerText.text = newText; //set text
    }

    public void SetIsCorrect(bool newBool) //get bool from other script
    {
        isCorrect = newBool; //set new boolean
    }

    public void SetButtonState(bool newBool)
    {
        isPressed = newBool;
    }

    private void addCorrectClicked() //counts how many times the user clicked correct buttons
    {
        correctClickedCount += 1;
    }

    private void addWrongClicked() //counts how many times the user clicked wrong buttons
    {
        wrongClickedCount += 1;
    }

    public void OnClick() //click function
    {
        if (isCorrect) //if it is the correct answer
        {
            pointCounter.increasePoints(correctPointAmount);
            addCorrectClicked();
            chestManager.disableChest();
            Debug.Log("Correct Answer");
        }
        else //if it is the wrong answer
        {
            timer.decreaseTime();
            addWrongClicked();
            Debug.Log("Wrong Answer");

            Debug.Log($"Wrong answers clicked: {wrongClickedCount}"); //debugging purposes
        }
    }

    public int getWrongClickedCount()
    {
        return wrongClickedCount;
    }

    public int getCorrectClickedCount()
    {
        return correctClickedCount;
    }


}
