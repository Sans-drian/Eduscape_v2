using UnityEngine;
using UnityEditor;
using System.IO;

public class TSVtoSO 
{
    private static string questionsTSVPath = "/Editor/TSVs/QuestionExamplesCS.txt";
    private static int numberOfAnswer = 4;

    [MenuItem("Utilities/Generate Questions")]

    public static void GeneratePhrases()
    {
        Debug.Log("Generated Questions");
        string[] allLines = File.ReadAllLines(Application.dataPath + questionsTSVPath);

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

        AssetDatabase.SaveAssets();
    }


}
