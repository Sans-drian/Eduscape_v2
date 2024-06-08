using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.Events;
using UnityEditor;

public class DatabaseMethods : MonoBehaviour
{
    [Serializable]
    public class StringEvent : UnityEvent<string> {} //create custom unity event class which can take in a string argument
    public UnityEvent sendErrorFileName; //send event when column from table is not found
    public UnityEvent runGame; //send event to start game
    public StringEvent contToTSVScript;


    // the 2 unity events below will be used in testing the connection string
    public UnityEvent sendMsgTrue;
    public UnityEvent sendMsgFalse;


    //private static string fileLocation = "/Editor/TSVs";
    private static string assetLocation = "Assets/Resources/Questions/";
    private string fileName; //string variable fileName holds the name of the question file that will be searched in the table of the database
    
    /*
    ============== CONNECTION STRING BELOW, PLEASE SET THIS CORRECTLY TO PROPERLY CONNECT TO YOUR DATABASE =================================
    */
    private string connectionString;

    private string inputServer;
    private string inputDatabase;
    private string inputUser;
    private string inputPassword;


    void Start()
    {
        connectionString = DatabaseManager.Instance.connectionString; //get the connection string from the DatabaseManager singleton script
    }
    




    // Set methods for connection input strings
    public void setInputServer(string inputServer)
    {
        this.inputServer = inputServer;
    }
    public void setInputDatabase(string inputDatabase)
    {
        this.inputDatabase = inputDatabase;
    }
    public void setInputUser(string inputUser)
    {
        this.inputUser = inputUser;
    }  
    public void setInputPassword(string inputPassword)
    {
        this.inputPassword = inputPassword;
    }

    public void setAndRunConnectionString()
    {
        UpdateConnectionString(inputServer, inputDatabase, inputUser, inputPassword);
        bool connectedDatabase = TestConnection();

        if (connectedDatabase) //if connnection is set
        {
            sendMsgTrue.Invoke();
            //Debug.Log("This connection string works and is set!");
        }
        else //if connection is not set
        {
            sendMsgFalse.Invoke();
            //Debug.LogError("This connection string doesn't work!");
        }
    }

    private void UpdateConnectionString(string userInputServer, string userInputDatabase, string userInputUser, string userInputPassword)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        builder.Server = userInputServer;
        builder.Database = userInputDatabase;
        builder.UserID = userInputUser;
        builder.Password = userInputPassword;

        // Update the existing connection string
        DatabaseManager.Instance.connectionString = builder.ConnectionString;
        connectionString = DatabaseManager.Instance.connectionString;
        Debug.Log($"Updated connection string: {DatabaseManager.Instance.connectionString}");
    }

    private bool TestConnection()
    {
        try
        {
            using (var connection = new MySqlConnection(DatabaseManager.Instance.connectionString))
            {
                connection.Open();
                DatabaseManager.Instance.isDatabaseOn = true;
                return true; // Connection successful
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions (e.g., invalid credentials, server down)
            Console.WriteLine($"Error connecting to the database: {ex.Message}");
            DatabaseManager.Instance.isDatabaseOn = false;
            return false; // Connection failed
        }
    }





    public void createQuestionList()
    {
        if (DatabaseManager.Instance.isDatabaseOn) //check if isDatabaseOn is true
        {
            CreateQuestionDataFromDatabase();
        }
        else
        {
            Debug.LogWarning("There was a problem connecting to the database. The game will now search question files from its own asset folder");
        }

    }

    public void setFileName(string input) //takes input of whatever player's file name input is and set it to the fileName string
    {
        fileName = input;
    }

    public void saveDataToDatabase()
    {   

        if (DatabaseManager.Instance.isDatabaseOn)
        {
            //get the strings of the playerprefs and set them into a temporary string variable
            string playerName = PlayerPrefs.GetString("PlayerName");
            string avgAnsAcc = PlayerPrefs.GetString("AvgCalcAns");
            string elapsedTime = PlayerPrefs.GetString("ElapsedTime");
            string questionList = PlayerPrefs.GetString("QListName");
            string dateTime = PlayerPrefs.GetString("LocalTime");

            InsertPlayerSessionResult(playerName, avgAnsAcc, elapsedTime, questionList, dateTime);
        }
        else
        {
            Debug.LogWarning("There was a problem connecting to the database ealier. The game will not try to query entry into the table of the database");
        }

    }

    // Method to retrieve data from the database
    public List<QuestionData> GetQuestionDataFromDatabase(string valueToSearch)
    {
        List<QuestionData> databaseRows = new List<QuestionData>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseManager.Instance.connectionString))
            {
                connection.Open();

                // Execute your SQL query
                string query = $"SELECT * FROM questionlisttable WHERE FILENAME = @columnname";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@columnname", valueToSearch);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Create a new QuestionData instance for each row
                            QuestionData rowData = ScriptableObject.CreateInstance<QuestionData>();
                            rowData.questionNum = reader.GetString("QUESTIONNUMBER");
                            rowData.question = reader.GetString("QUESTION");
                            rowData.category = reader.GetString("CATEGORY");

                            rowData.answers = new string[4];
                            rowData.answers[0] = reader.GetString("CORRECT");
                            rowData.answers[1] = reader.GetString("WRONG1");
                            rowData.answers[2] = reader.GetString("WRONG2");
                            rowData.answers[3] = reader.GetString("WRONG3");

                            databaseRows.Add(rowData);
                        }

                        Debug.Log($"Row found in the database for column: {valueToSearch}");
                    }
                    else
                    {
                        Debug.LogError($"No rows found in the database for column: {valueToSearch}");
                        sendErrorFileName.Invoke(); //send event when column from table is not found
                        return null;
                    }
                    
                }
            }
            
        }
        catch (MySqlException e)
        {
            Debug.LogError($"Error fetching data from the database: {e.Number}: {e.Message}");
            DatabaseManager.Instance.isDatabaseOn = false;
            contToTSVScript.Invoke(fileName); //run the method to search question list from TSVtoSO, once
            return null;
        }

        return databaseRows;
    }

    public void CreateQuestionDataFromDatabase()
    {
        // Create a new list to store the retrieved data
        List<QuestionData> databaseRows = new List<QuestionData>();

        // Delete existing assets (if any) before creating new ones
        string assetFolderPath = assetLocation; // Adjust the folder path as needed
        string[] existingAssetPaths = AssetDatabase.FindAssets("t:Question", new[] { assetFolderPath });
        foreach (var existingAssetPath in existingAssetPaths)
        {
            AssetDatabase.DeleteAsset(existingAssetPath);
        }

        // Retrieve data from the database
        databaseRows = GetQuestionDataFromDatabase(fileName);

        if (databaseRows != null)
        {
            foreach (var row in databaseRows)
            {
                // Create a new QuestionData instance
                QuestionData questionData = ScriptableObject.CreateInstance<QuestionData>();
                questionData.question = row.question;
                questionData.category = row.category;
                questionData.answers = row.answers;

                // Save the instance as an asset
                string newAssetPath = $"{assetFolderPath}Question{row.questionNum}.asset";
                AssetDatabase.CreateAsset(questionData, newAssetPath);
            }
            
            Debug.Log("Questions created from database data! (Ran from DatabaseMethods.cs)");
            runGame.Invoke(); //send event to start game
            
            // Refresh the AssetDatabase
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("database is null");
        }
    }


    public void InsertPlayerSessionResult(string playerName, string avgAnsAcc, string elapsedTime, string questionList, string dateTime)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseManager.Instance.connectionString))
            {
                connection.Open();

                // This SQL query is to insert an entry into the table
                string query = $"INSERT INTO playersessionresult (PLAYER_NAME, AVERAGE_ANSWER_ACCURACY, ELAPSED_TIME, QUESTION_LIST, DATE_TIME) " +
                   "VALUES (@playerName, @avgAnsAcc, @elapsedTime, @questionList, @dateTime)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Set parameter values
                    cmd.Parameters.AddWithValue("@playerName", playerName);
                    cmd.Parameters.AddWithValue("@avgAnsAcc", avgAnsAcc);
                    cmd.Parameters.AddWithValue("@elapsedTime", elapsedTime);
                    cmd.Parameters.AddWithValue("@questionList", questionList);
                    cmd.Parameters.AddWithValue("@dateTime", dateTime);

                    // Execute the query
                    cmd.ExecuteNonQuery();
                }
                Debug.Log("Insert to database successful!");
            }
        }
        catch (MySqlException e)
        {
            Debug.LogError($"Error fetching data from the database: {e.Message}");
            //sendErrorDatabase.Invoke(e.Message); //send event when database is not found
        }
    }
}
