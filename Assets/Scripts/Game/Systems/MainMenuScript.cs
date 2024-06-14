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
    public StringEvent searchQListAndRun;
    public UnityEvent setPlayerPrefsInfo;


    private string errorTxtColor = "#FF6666"; //error texts will have this color
    private string correctTxtColor = "#B0FF58"; //non-error texts will have this color
    private int charLimit = 20;


    // Input fields below to get input for play game screen
    [SerializeField]
    private TMP_InputField inputUserName;
    [SerializeField]
    private TMP_InputField inputFileName;
    [SerializeField]
    private TextMeshProUGUI questionSetFromText;
    [SerializeField]
    private TextMeshProUGUI debugTextPlayMenu;
    [SerializeField]
    private Button startGameBttn;
    [SerializeField]
    private Button clearUserNameInputButton;
    [SerializeField]
    private Button clearFileNameInputButton;
    [SerializeField]
    private Button clearAllInputButton;
    [SerializeField]
    private Button backButton;
    


    // Input fields below to get input to build a connection string
    [SerializeField]
    private TMP_InputField inputServer;
    [SerializeField]
    private TMP_InputField inputDatabase;
    [SerializeField]
    private TMP_InputField inputUser;
    [SerializeField]
    private TMP_InputField inputPassword;
    [SerializeField]
    private TextMeshProUGUI debugTextCSMenu;

    public StringEvent setInputServer;
    public StringEvent setInputDatabase;
    public StringEvent setInputUser;
    public StringEvent setInputPassword;
    public UnityEvent runConnectionTest;
    
    
    public DatabaseMethods databaseMethods;

    private string input; //input string variable for the input field




    // Start is called before the first frame update
    void Start()
    {
        //set the debug texts in the scene to input
        debugTextPlayMenu.text = ""; 
        questionSetFromText.text = "";

        debugTextCSMenu.text = "";
    }

    public void GetInputAndSetQ()
    {
        if (inputUserName.text.Length > charLimit) //check if the name inputted by user is over 20 characters
        {
            //throw error
            //Debug.LogWarning("input user name is over 20 characters, please try again!");
            debugTextPlayMenu.text = $"<color={errorTxtColor}>Player name input is over 20 characters! Please re-enter a new name.</color>";
        }
        else if (inputUserName.text == "") //check if the name inputted by user is empty
        {
            //throw error
            //Debug.LogWarning("empty input field name, please enter a name.");
            debugTextPlayMenu.text = $"<color={errorTxtColor}>Player name input is empty! Please enter a name.</color>";
        }
        else //if name inputted is less than 20 characters and not empty
        {
            makeQuestionList();
        }
    }

    public void makeQuestionList()
    {
        bool currentBoolDTB = DatabaseManager.Instance.isDatabaseOn; //get the current bool for isDatabaseOn
        input = inputFileName.text; //place input from text field

        if (currentBoolDTB) //if isDatabaseOn is true (or if its on)
        {
            databaseMethods.setFileName(input); //set the fileName (columnname)

            databaseMethods.createQuestionList(); //start method to create question list (from DatabaseMethods)
            // this method includes a unity event called runGame that is invoked when it successfully creates the questions taken from the database. 
            // the runGame invoke is invoking the method callRunGame() that exists in this script.
        }
        else
        {
            searchQListAndRun.Invoke(input); //this invokes an event which takes the input as an argument and runs runGenerateQuestions from TSVtoSOMono script
        }
    }



    // ================================================================



    public void testDatabaseConnection()
    {
        setInputServer.Invoke(inputServer.text);
        setInputDatabase.Invoke(inputDatabase.text);
        setInputUser.Invoke(inputUser.text);
        setInputPassword.Invoke(inputPassword.text);
        
        runConnectionTest.Invoke();
    }

    public void displayConnectedMsg()
    {
        Debug.Log("Connection string set and found your database!");
        debugTextCSMenu.text = $"<color={correctTxtColor}>Connection string set and found your database!</color>";
    }


    public void displayNotConnectedMsg()
    {
        Debug.LogError("Connection string inputted cannot find a database/server..");
        debugTextCSMenu.text = $"<color={errorTxtColor}>Connection string inputted cannot connect to a database/server.. Game will not use take from or set to database.</color>";
    }




    // ================================================================



    public void IfMissingDatabase(string error) //method called when sendErrorDatabase event is invoked (from DatabaseMethods)
    {
        //Debug.LogError($"Error code from MySqlException: {error}");
        debugTextPlayMenu.text = $"<color={errorTxtColor}> Error from mySQL: {error} </color>"; //change debug text to display error message
    }

    public void IfMissingFileName() //method called when sendErrorFileName event is invoked (from DatabaseMethods)
    {
        if (input == "")
        {
            //Debug.LogError("Input field is empty, please enter text.");
            debugTextPlayMenu.text = $"<color={errorTxtColor}>Input field is empty, please enter text.</color>";
        }
        else
        {
            //Debug.LogError($"Error finding the {input}! Please check if fileName is correct!");
            debugTextPlayMenu.text = $"<color={errorTxtColor}>Error finding {input}! Please check if file name inputted is correct!</color>";
        }
    }

    public void callRunGame() //method called when runGame event is invoked (from DatabaseMethods)
    {
        //invoke the events and passes in the corresponding string arguments
        setNameString.Invoke(inputUserName.text);
        setFileNameString.Invoke(inputFileName.text);
        
        if (DatabaseManager.Instance.isDatabaseOn)
        {
            questionSetFromText.text = $"<color={correctTxtColor}>Questions set from Database!</color>";
        }
        else
        {
            questionSetFromText.text = $"<color={correctTxtColor}>Questions set from existing game files!</color>";
        }
        
        StartCoroutine (runGame());
    }

    private IEnumerator runGame() //enum type method to run game (which allows for the use of WaitForSeconds)
    {
        Debug.Log("runGame method has been reached!");
        setPlayerPrefsInfo.Invoke();
        string tempFileNameVar = inputFileName.text;

        disableButtonsPlayMenu();

        /*
        TIMER FUNCTION (5 seconds):
        This implementation of the timer countdown is very bad, but works for now, please improve upon it in the future, to whom ever sees this.
        */
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 5...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 4...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 3...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 2...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 1...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Name and Questions from {tempFileNameVar} are set! Starting game in 0...</color>";
        yield return new WaitForSeconds(1);
        debugTextPlayMenu.text = $"<color={correctTxtColor}>Starting game!</color>";
        yield return new WaitForSeconds(1);

        Debug.Log("Changing game scene...");
        SceneManager.LoadScene("Game"); // change scene to game
        
    }

    private void disableButtonsPlayMenu() //method to disable interaction for buttons in play menu
    {
        startGameBttn.interactable = false; 
        clearUserNameInputButton.interactable = false;
        clearFileNameInputButton.interactable = false;
        clearAllInputButton.interactable = false;
        backButton.interactable = false;
    }


    // ==================================================================


    public void ExitGame() //exits game. This method is only available to be called for the non WebGL build
    {
        Application.Quit();
    }


    // PLAY MENU CLEAR BUTTON METHODS =================================================================

    public void ClearTextsPlayMenu()
    {
        debugTextPlayMenu.text = "";
        questionSetFromText.text = "";

        inputUserName.text = "";
        inputFileName.text = "";
        //Debug.Log("Input fields and other texts of Play menu is cleared!");
    }

    //Clear methods below are for the clear buttons
    public void ClearUserNameInput()
    {
        inputUserName.text = "";
    }

    public void ClearFileNameInput()
    {
        inputFileName.text = "";
    }

    public void ClearAllInputPlayMenu()
    {
        inputUserName.text = "";
        inputFileName.text = "";
    }




    // CONNECTION STRING MENU CLEAR BUTTON METHODS =================================================================



    public void ClearTextsCSMenu()
    {
        debugTextCSMenu.text = "";
        inputServer.text = "";
        inputDatabase.text = "";
        inputUser.text = "";
        inputPassword.text = "";

        //Debug.Log("Input fields and Debug Text of Connection String menu is cleared!");
    }

    //Clear methods below are for the clear buttons
    public void ClearServerInput()
    {
        inputServer.text = "";
    }

    public void ClearDatabaseInput()
    {
        inputDatabase.text = "";
    }

    public void ClearUserInput()
    {
        inputUser.text = "";
    }

    public void ClearPasswordInput()
    {
        inputPassword.text = "";
    }

    public void ClearAllInputCSMenu()
    {
        inputServer.text = "";
        inputDatabase.text = "";
        inputUser.text = "";
        inputPassword.text = "";
    }
}
