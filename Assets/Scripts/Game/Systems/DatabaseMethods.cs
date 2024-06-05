using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.Events;

public class DatabaseMethods : MonoBehaviour
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string> {} //create custom unity event class which can take in a string argument
    public StringEvent sendErrorDatabase; //send event when database is not found 
    public UnityEvent sendErrorFileName; //send event when column from table is not found
    public UnityEvent runGame; //send event to start game


    //private static string fileLocation = "/Editor/TSVs";
    private static string assetLocation = "Assets/Resources/Questions/";
    private string fileName; //string variable fileName holds the name of the question file that will be searched in the table of the database
    private string connectionString = "Server=localhost;Database=eduscapeqdatabasetest;User=root;Password=;";
    // Start is called before the first frame update

    public void createQuestionList()
    {
        CreateQuestionDataFromDatabase();
    }

    public void setFileName(string input)
    {
        fileName = input;
    }

    // Method to retrieve data from the database
    public List<QuestionData> GetQuestionDataFromDatabase(string valueToSearch)
    {
        List<QuestionData> databaseRows = new List<QuestionData>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
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
                        //Debug.LogError($"No rows found in the database for column: {valueToSearch}");
                        sendErrorFileName.Invoke(); //send event when column from table is not found
                        return null;
                    }
                    
                }
            }
            
        }
        catch (MySqlException e)
        {
            //Debug.LogError($"Error fetching data from the database: {e.Message}");
            sendErrorDatabase.Invoke(e.Message); //send event when database is not found
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
        string[] existingAssetPaths = UnityEditor.AssetDatabase.FindAssets("t:Question", new[] { assetFolderPath });
        foreach (var existingAssetPath in existingAssetPaths)
        {
            UnityEditor.AssetDatabase.DeleteAsset(existingAssetPath);
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
                UnityEditor.AssetDatabase.CreateAsset(questionData, newAssetPath);
            }
            
            Debug.Log("Questions created from database data! (Ran from DatabaseMethods.cs)");
            runGame.Invoke(); //send event to start game
            
            // Refresh the AssetDatabase
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("database is null");
        }
    }
}
