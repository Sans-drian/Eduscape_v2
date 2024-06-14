using UnityEngine;
using System.IO;
using UnityEngine.Events;

/*

Note: The 'Mono' in the class name is meant to differenciate between the non monobehavior version of this script in the Editor folder, which is now unused.

*/

public class TSVtoSOMono : MonoBehaviour
{
    public UnityEvent errMessage;
    public UnityEvent runGame;

    // the 3 static string below will be combined to create the fullPath string in GenerateQuestions()
    private static string baseFolderPath = Application.dataPath;
    private static string questionsTSVPath = "/Resources/TSVs/"; 
    private static string fileNameSearch;


    private static int numberOfAnswer = 4;

    private bool questionFound;

    void Awake()
    {
        questionFound = false;
    }

    public void runGenerateQuestions(string input) //called from DatabaseMethods
    {
        fileNameSearch = input; 
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

        TextAsset textAsset = Resources.Load<TextAsset>("TSVs/" + fileNameSearch);
        Debug.Log($"Loaded Asset: {textAsset}");

        if (textAsset != null) //check if filepath exists (in turn, checking if the player's input is correct) File.Exists(fullPath)
        {

            QuestionManager.Instance.questionList.Clear(); //clear the static list first to ensure questions don't get duplicated.
            //Debug.Log($"check existing asset path: {QuestionManager.Instance.questionList.Count}");


            string[] allLines = textAsset.text.Split('\n'); //File.ReadAllLines(fullPath);

            foreach (string s in allLines)
            {
                string[] splitData = s.Split('\t');

                // TSV (Table Separated Value) data format (the file will be .txt, but will work the same as a tsv file)
                // QUESTION NUMBER, QUESTION, CATEGORY, CORRECT ANSWER, WRONG ANSWER 1, WRONG ANSWER 2, WRONG ANSWER 3 [in that order]
                if (splitData.Length >= 7)
                {
                    QuestionData questionData = ScriptableObject.CreateInstance<QuestionData>();
                    questionData.question = splitData[1]; // 2nd column from tsv file
                    questionData.category = splitData[2];

                    // Initialize the array of answers
                    questionData.answers = new string[4];

                    for (int i = 0; i < numberOfAnswer; i++)
                    {
                        questionData.answers[i] = splitData[3 + i];
                    }

                    //add contents into the static list
                    QuestionManager.Instance.questionList.Add(questionData);

                }
                else
                {
                    Debug.LogError($"Invalid TSV format: {s}");
                }

            }


            Debug.Log($"Created {allLines.Length} assets.");

            questionFound = true;
        }
        else
        {
            errMessage.Invoke();
            Debug.Log($"Question file not found: {fileNameSearch}");
        }
        
    }
}
