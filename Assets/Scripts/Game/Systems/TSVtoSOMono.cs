using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Events;

public class TSVtoSOMono : MonoBehaviour
{
    public UnityEvent errMessage;
    public UnityEvent runGame;

    // the 3 static string below will be combined to create the fullPath string in GenerateQuestions()
    private static string baseFolderPath = Application.dataPath;
    private static string questionsTSVPath = "/Editor/TSVs/"; 
    private static string fileNameSearch;

    private static string assetLocation = "Assets/Resources/Questions/";

    private static int numberOfAnswer = 4;

    private bool questionFound;

    void Awake()
    {
        questionFound = false;
    }

    public void runGenerateQuestions(string input)
    {
        fileNameSearch = input + ".txt"; 
        GenerateQuestions();

        if (questionFound) //checks first if question is found and set, then run game.
        {
            Debug.Log("Run game is called from TSVtoSOMono");
            runGame.Invoke();
        }
        
    }

    
    public void GenerateQuestions()
    {
        Debug.Log("Generated Questions");
        string fullPath = baseFolderPath + questionsTSVPath + fileNameSearch;

        // Delete existing assets (if any) before creating new ones
        string assetFolderPath = assetLocation; // Adjust the folder path as needed
        string[] existingAssetPaths = AssetDatabase.FindAssets("t:QuestionData", new[] { assetFolderPath });
        
        
        foreach (var existingAssetPath in existingAssetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(existingAssetPath);
            AssetDatabase.DeleteAsset(assetPath);
        }
        
        Debug.Log($"check existing asset path: {existingAssetPaths.Length}");
        AssetDatabase.Refresh();

        if (File.Exists(fullPath)) //check if filepath exists (in turn, checking if the player's input is correct)
        {
            string[] allLines = File.ReadAllLines(fullPath);

            foreach (string s in allLines)
            {
                string[] splitData = s.Split('\t');

                // TSV (Table Separated Value) data format (the file will be .txt, but will work the same as a tsv file)
                // QUESTION NUMBER, QUESTION, CATEGORY, CORRECT ANSWER, WRONG ANSWER 1, WRONG ANSWER 2, WRONG ANSWER 3 [in that order]

                QuestionData questionData = ScriptableObject.CreateInstance<QuestionData>();
                questionData.question = splitData[1]; // 2nd column from tsv file
                questionData.category = splitData[2];

                // Initialize the array of answers
                questionData.answers = new string[4];

                for (int i = 0; i < numberOfAnswer; i++)
                {
                    questionData.answers[i] = splitData[3 + i];
                }

                // Grab the question number from column 0 of the txt file
                string questionNum = splitData[0];
                // Save this in the RESOURCES folder to load them later by script
                AssetDatabase.CreateAsset(questionData, $"Assets/Resources/Questions/Question{questionNum}.asset");
            }

            string assetFolderPathw = assetLocation; // Adjust the folder path as needed
            string[] existingAssetPaths2 = AssetDatabase.FindAssets("t:QuestionData", new[] { assetFolderPathw });
            Debug.Log($"check existing asset path: {existingAssetPaths2.Length}");

            questionFound = true;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            errMessage.Invoke();
            Debug.Log($"Question file not found: {fileNameSearch}");
        }
    }
}
