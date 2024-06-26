using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;
using UnityEngine.Events;

public class AnswerButton : MonoBehaviour
{
    public UnityEvent correctPressed;
    public UnityEvent wrongPressed;

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
    public CalculateAnsAcc calculateAnsAcc; //calculate answer accuracy
    public Chest chest;

    private int correctPointAmount = 1;

    void Awake()
    {
        chest = FindObjectOfType<Chest>();
    }
    
    void Start()
    {
        /* debugging purposes
        if (questionSetup != null)
        {
            Debug.Log("questionSetup is not null");
        }
        else
        {
            Debug.LogError("questionSetup is null");
        }
        */
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

    public void setButtonID()
    {
        //Debug.Log("setButtonID() called");
        questionSetup.buttonID = buttonID;
    }

    public void OnClick() //click function
    {
        setButtonID();
        calculateAnsAcc.setCurrentIntArray();  
        questionSetup.updateBoolAddCount();

        
        if (isCorrect) //if it is the correct answer
        {
            correctPressed.Invoke();
            //chest.isAnswered();
            pointCounter.increasePoints(correctPointAmount);
            calculateAnsAcc.storeIndvAns(); //call method to calculate individual answer accuracy and store it in the dict (see CalculateAnsAcc script)
            chestManager.disableChest();
            Debug.Log("Correct Answer");


            /*debugging purposes
            foreach (var kvp in calculateAnsAcc.qClickedCount)
            {
                int arrayNameTest = kvp.Key;
                int[] arrayTest = kvp.Value;

                Debug.Log($"Array '{arrayNameTest}': {string.Join(", ", arrayTest)}");
            }
            */
        }
        else //if it is the wrong answer
        {
            wrongPressed.Invoke();
            timer.decreaseTime();
            Debug.Log("Wrong Answer");

            if (!isPressed)
            {
                isPressed = true;
                Debug.Log("Setting to button isPressed to true");
                
            }
            else
            {
                Debug.Log("Button isPressed is already true");
            }
        }
    }

}
