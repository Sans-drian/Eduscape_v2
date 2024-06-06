using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour
{   
    [Serializable]
    public class StringEvent : UnityEvent<string> {} //create custom unity event class which can take in a string argument
    
    public StringEvent setNameString;
    public StringEvent setFileNameString;
    public UnityEvent setPlayerPrefsInfo;

    [SerializeField]
    private TMP_InputField inputUserName;
    [SerializeField]
    private TMP_InputField inputFileName;
    [SerializeField]
    private TextMeshProUGUI debugText;
    [SerializeField]
    private Button startGameBttn;
    private string errorTxtColor = "#FF6666"; //error texts will have this color
    private string correctTxtColor = "#B0FF58"; //non-error texts will have this color
    private int charLimit = 20;
    
    public DatabaseMethods databaseMethods;

    private string input; //input string variable for the input field

    // Start is called before the first frame update
    void Start()
    {
        debugText.text = ""; //set the debug text in the scene to input
    }

    public void GetInputAndSetQ()
    {
        if (inputUserName.text.Length > charLimit) //check if the name inputted by user is over 20 characters
        {
            //throw error
            Debug.LogWarning("input user name is over 20 characters, please try again!");
            debugText.text = $"<color={errorTxtColor}>Player name input is over 20 characters! Please re-enter a new name.</color>";
        }
        else if (inputUserName.text == "") //check if the name inputted by user is empty
        {
            //throw error
            Debug.LogWarning("empty input field name, please enter a name.");
            debugText.text = $"<color={errorTxtColor}>Player name input is empty! Please enter a name.</color>";
        }
        else //if name inputted is less than 20 characters and not empty
        {
            input = inputFileName.text; //place input from text field
            databaseMethods.setFileName(input); //set the fileName (columnname)

            databaseMethods.createQuestionList(); //start method to create question list (from DatabaseMethods)
        }
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

    public void callRunGame() //method called when runGame event is invoked (from DatabaseMethods)
    {
        //invoke the events and passes in the corresponding string arguments
        setNameString.Invoke(inputUserName.text);
        setFileNameString.Invoke(inputFileName.text);

        StartCoroutine (runGame());
    }

    private IEnumerator runGame() //enum type method to run game (which allows for the use of WaitForSeconds)
    {
        Debug.Log("runGame method has been reached!");
        setPlayerPrefsInfo.Invoke();
        startGameBttn.interactable = false; //disable interaction for the button

        /*
        TIMER FUNCTION (5 seconds):
        This implementation of the timer countdown is very bad, but works for now, please improve upon it in the future, to whom ever sees this.
        */
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 5...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 4...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 3...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 2...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 1...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Name and Questions set! Starting game in 0...</color>";
        yield return new WaitForSeconds(1);
        debugText.text = $"<color={correctTxtColor}>Starting game!</color>";
        yield return new WaitForSeconds(1);

        Debug.Log("Changing game scene...");
        SceneManager.LoadScene("Game"); // change scene to game
        
    }
}
