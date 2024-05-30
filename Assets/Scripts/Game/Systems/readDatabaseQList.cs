using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;

public class readDatabaseQList : MonoBehaviour
{
    //private static string fileLocation = "/Editor/TSVs";
    private static string assetLocation = "Assets/Resources/Questions/";
    public string fileName;
    //private static int numberOfAnswer = 4;
    private string connectionString = "Server=localhost;Database=eduscapeqdatabasetest;User=root;Password=;";
    // Start is called before the first frame update

    public void createQuestionList()
    {
        CreateQuestionDataFromDatabase();
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
                }
            }
            SceneManager.LoadScene("Game");
        }
        catch (MySqlException e)
        {
            Debug.LogError($"Error fetching data from the database: {e.Message}");
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

        Debug.Log("Questions created from database data");
        // Refresh the AssetDatabase
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
}
