using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class getInput : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextMeshProUGUI debugText;
    
    public DatabaseMethods databaseMethods;

    private TimeSpan timeLeft = new TimeSpan(0, 0, 3);

    private string input;
    // Start is called before the first frame update
    
    void Start()
    {
        
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
    }

    public void IfMissingFileName() //method called when sendErrorFileName event is invoked (from DatabaseMethods)
    {
        if (input == "")
        {
            Debug.LogError("Input field is empty, please enter text.");
        }
        else
        {
            Debug.LogError($"Error finding the {input}! Please check if fileName is correct!");
        }
    }

    public void callRunGame()
    {
        StartCoroutine (runGame());
    }

    private IEnumerator runGame() //method called when runGame event is invoked (from DatabaseMethods)
    {
        Debug.Log("YIPPEEE! Time to run game!");
        yield return new WaitForSeconds(3);
        Debug.Log("3 Seconds have passed...");
        
    }
}
