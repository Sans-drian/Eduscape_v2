using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CalculateAnsAcc : MonoBehaviour
{
    public QuestionSetup questionSetup;
    public ChestManager chestManager;
    
    public Dictionary<int, int[]> qClickedCount = new Dictionary<int, int[]>(); //question clicked count dictionary
    public Dictionary<int, float[]> indvAnsAcc = new Dictionary<int, float[]>(); //individual answer accuracy dictionary

    int[] currentQuestion;


    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeQIntArrays();
        Debug.Log($"Dictionary count: {qClickedCount.Count}");
        Debug.Log($"questionCount from questionSetup script is {questionSetup.questionsCount}");
        
        /*

        For each loop for the purposes of debugging
        (Display all of the key value pairs in the dicitonaries)

        foreach (var kvp in qClickedCount) 
        {
            int arrayNameTest = kvp.Key;
            int[] arrayTest = kvp.Value;

            Debug.Log($"Array qClickedCount '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }

        foreach (var kvp in indvAnsAcc)
        {
            int arrayNameTest = kvp.Key;
            int[] arrayTest = kvp.Value;

            Debug.Log($"Array indvAnsAcc '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }
        */

        //Debug.Log($"The count of questions is {questionSetup.questionsCount}"); //debugging purposes
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setCurrentIntArr()
    {
       currentQuestion = GetIndvArray(questionSetup.QuestionIndex, qClickedCount);
    }

    public void addCount()
    {
        currentQuestion[0] += 1;
    }
    
    public void InitializeQIntArrays() //create int arrays to hold button states and individual answer accuracies
    {
        for (int i = 0; i < questionSetup.questionsCount; i++)
        {
            int arrayName = i;
            int[] newArrayI = new int[1]; // Change the size as needed
            float[] newArrayF = new float[1]; 

            // Set all values to false
            for (int j = 0; j < newArrayI.Length; j++)
            {
                newArrayI[j] = 0;
                newArrayF[j] = 0;
            }

            // Add the array to the dictionary
            AddToQClicked(arrayName, newArrayI);
            AddToAnsQuest(arrayName, newArrayF);
            //Debug.Log($"dict set {i}");
            
        }
    }

    private void AddToQClicked(int arrayName, int[] array) //method to add the array into the question clicked count dictionary
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
    private void AddToAnsQuest(int arrayName, float[] array) //method to add the array into the individual answer accuracy dictionary
    {
        if (!indvAnsAcc.ContainsKey(arrayName))
        {
            indvAnsAcc.Add(arrayName, array);
        }
        else
        {
            Debug.LogWarning($"Array '{arrayName}' already exists in the dictionary.");
        }

    }

    private T[] GetIndvArray<T>(int arrayValue, Dictionary<int, T[]> dictionary)
    {
        if (dictionary.TryGetValue(arrayValue, out T[] array))
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
    
    
    public void storeIndvAns() //method to store the calculated answer accuracy of a question into indvAnsAcc dictionary
    {
        float calculatedNum = calcIndvAnsAc(questionSetup.QuestionIndex);  //calculate question answer accuracy based on the current question index
        float[] currentArray = GetIndvArray(questionSetup.QuestionIndex, indvAnsAcc);

        currentArray[0] = calculatedNum;

        foreach (var kvp in indvAnsAcc)
        {
            int arrayNameTest = kvp.Key;
            float[] arrayTest = kvp.Value;

            Debug.Log($"Array '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }
    }

    private float calcIndvAnsAc(int array) //method that calculates the answer accuracy of a question
    {
        float totalClicked = 0;

        int[] currentArray = GetIndvArray(array, qClickedCount); //find array matching from qClickedCount to the current question (or int of the current question)
        Debug.Log($"This question array has {currentArray[0]} clicks");
        totalClicked = currentArray[0];

        float calcIndvAns = 1 / totalClicked;

        /*
        Convert 25% to 0% WITHOUT using an if-conditional
        */
        float cutoff = 1-(int) (calcIndvAns+0.7f); // if 25% return 1, else return 0
        float newAccu = calcIndvAns - (0.25f*cutoff);
        
        Debug.Log($"calculated answer accuracy for question {array + 1}: {newAccu} (before, it was {calcIndvAns})");
        return newAccu;
    }


    public int calcTotalAnsAcc()
    {
        int calcTotalAns = 0;
        
        /*
        GameObject interactingChest = chestManager.chests[chestManager.clickedObjectIndex]; // Get the specific GameObject
        var chest = interactingChest.GetComponent<Chest>(); // place the component into the variable chest

        if (!chest.isAnswered)
        {
            Debug.Log("Chest has yet to be answered to calculate answer accuracy of question");
        }
        else
        {
            int totalClicked = currentQuestion[0];
            calcIndvAnsAc(totalClicked);
            Debug.Log("Answer accuracy of current question has been calculated!");
        }

        foreach (var kvp in qClickedCount) 
        {      
            int[] arrayClickedCount = kvp.Value;
            int arrayIndex = arrayClickedCount[0];

            calcIndvAnsAc(kvp.Value, arrayIndex);

        }
        */

        return calcTotalAns;
    }


}
