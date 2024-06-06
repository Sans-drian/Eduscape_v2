using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI debugText;
    private string errorTxtColor = "#FF6666"; //error texts will have this color
    private string correctTxtColor = "#B0FF58"; //non-error texts will have this color
    
    public DatabaseMethods databaseMethods;


    //private TimeSpan timeLeft = new TimeSpan(0, 0, 3);

    private string input;
    // Start is called before the first frame update
    
    void Start()
    {
        debugText.text = "";        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInputAndSetQ()
    {
        input = inputField.text; //place input from text field
        databaseMethods.setFileName(input); //set the fileName (columnname)

        databaseMethods.createQuestionList(); //start method to create question list (from DatabaseMethods)
    }

    public void IfMissingDatabase(string error) //method called when sendErrorDatabase event is invoked (from DatabaseMethods)
    {
        Debug.LogError($"Error code from MySqlException: {error}");
        debugText.text = $"<color={errorTxtColor}> Error from mySQL: {error} </color>"; //change debug text to display error message
    }

    public void IfMissingFileName() //method called when sendErrorFileName event is invoked (from DatabaseMethods)
    {
        if (input == "")
        {
            Debug.LogError("Input field is empty, please enter text.");
            debugText.text = $"<color={errorTxtColor}>Input field is empty, please enter text.</color>";
        }
        else
        {
            Debug.LogError($"Error finding the {input}! Please check if fileName is correct!");
            debugText.text = $"<color={errorTxtColor}>Error finding {input}! Please check if file name inputted is correct!</color>";
        }
    }

    public void callRunGame()
    {
        StartCoroutine (runGame());
    }

    private IEnumerator runGame() //method called when runGame event is invoked (from DatabaseMethods)
    {
        Debug.Log("YIPPEEE! Time to run game!");
        debugText.text = $"<color={correctTxtColor}>Questions set! Starting game in 3...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Questions set! Starting game in 2...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Questions set! Starting game in 1...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Starting game!</color>";
        yield return new WaitForSeconds(1);
        Debug.Log("4 Seconds have passed...");
        
    }
}
