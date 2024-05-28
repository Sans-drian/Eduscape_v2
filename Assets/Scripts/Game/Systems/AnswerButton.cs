using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;

public class AnswerButton : MonoBehaviour
{
    [SerializeField]
    private bool isCorrect; //know if this button is the correct answer
    [SerializeField]
    private bool isPressed; //know if this button has been pressed or not
    [SerializeField]
    private int buttonID; //id associated with the button
    [SerializeField]
    private TextMeshProUGUI answerText; //the answer text that will be changed

    [SerializeField]
    private PointCounter pointCounter;
    [SerializeField]
    private Timer timer;

    public ChestManager chestManager;

    public QuestionSetup questionSetup;

    private int wrongClickedCount;
    private int correctClickedCount;

    private int correctPointAmount = 1;

    void Start()
    {
        //questionSetup = GetComponent<QuestionSetup>();
    }

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

    public void SetButtonID(int newID)
    {
        buttonID = newID;
    }

    public void OnClick() //click function
    {
        setButtonID();
        questionSetup.updateArrayBool();
        
        if (isCorrect) //if it is the correct answer
        {
            pointCounter.increasePoints(correctPointAmount);
            //addCorrectClicked();
            chestManager.disableChest();
            Debug.Log("Correct Answer");

            Debug.Log($"Correct answers clicked: {correctClickedCount}"); //debugging purposes
        }
        else //if it is the wrong answer
        {
            timer.decreaseTime();
            Debug.Log("Wrong Answer");

            if (!isPressed)
            {
                //addWrongClicked();
                isPressed = true;
                Debug.Log("Button has not been pressed. Adding 1 to buttons pressed");
                
            }
            else
            {
                Debug.Log("Button has been pressed. Will not add 1 to buttons pressed");
            }

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

    public void setButtonID()
    {
        questionSetup.buttonID = buttonID;
    }


}
