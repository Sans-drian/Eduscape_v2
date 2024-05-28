using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAnsAcc : MonoBehaviour
{
    [SerializeField]
    int correctClickedCount = 0;
    [SerializeField]
    int incorrectClickedCount = 0;
    

    public QuestionSetup questionSetup;
    
    private Dictionary<int, int[]> qClickedCount = new Dictionary<int, int[]>();

    int[] currentQuestion;


    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeButtonBoolArrays();
        Debug.Log($"Dictionary count: {qClickedCount.Count}");
        Debug.Log($"questionCount from questionSetup script is {questionSetup.questionsCount}");

        foreach (var kvp in qClickedCount)
        {
            int arrayNameTest = kvp.Key;
            int[] arrayTest = kvp.Value;

            Debug.Log($"Array '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }

        //Debug.Log($"The count of questions is {questionSetup.questionsCount}"); //debugging purposes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentIntArr()
    {
       currentQuestion = GetBooleanArray(questionSetup.QuestionIndex);
    }

    public void addCorrectClicked()
    {
        correctClickedCount += 1;
    }

    public void addWrongClicked()
    {
        incorrectClickedCount += 1;
    }
    
    public void InitializeButtonBoolArrays() //create int arrays to hold button states
    {
        for (int i = 0; i < questionSetup.questionsCount; i++)
        {
            int arrayName = i;
            int[] newArray = new int[1]; // Change the size as needed

            // Set all values to false
            for (int j = 0; j < newArray.Length; j++)
            {
                newArray[j] = 0;
            }

            // Add the array to the dictionary
            AddIntArray(arrayName, newArray);
            //Debug.Log($"dict set {i}");
            
        }
    }

    private void AddIntArray(int arrayName, int[] array) //method to add the array into the dictionary
    {
        if (!qClickedCount.ContainsKey(arrayName))
        {
            qClickedCount.Add(arrayName, array);
        }
        else
        {
            Debug.LogWarning($"Array '{arrayName}' already exists in the dictionary.");
        }

    }

    private int[] GetBooleanArray(int arrayValue)
    {
        if (qClickedCount.TryGetValue(arrayValue, out int[] array))
        {
            Debug.Log($"Array {arrayValue} found in dictionary");
            return array;
        }
        else
        {
            Debug.LogError($"Array '{arrayValue}' not found in the dictionary.");
            return null;
        }
    }
    
}
