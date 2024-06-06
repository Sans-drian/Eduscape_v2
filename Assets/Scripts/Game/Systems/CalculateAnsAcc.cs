using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CalculateAnsAcc : MonoBehaviour
{
    [Serializable]
    public class StringEvent : UnityEvent<string> {} //create custom unity event class which can take in a string argument
    public StringEvent setAvgAnsAcc;

    public QuestionSetup questionSetup;
    public ChestManager chestManager;
    
    public Dictionary<int, int[]> qClickedCount = new Dictionary<int, int[]>(); //dict to hold the amount of clicks for each question
    public Dictionary<int, float[]> indvAnsAcc = new Dictionary<int, float[]>(); //dict to hold every question's calculated answer accuracy

    int[] currentQuestion; //int array to store the array from

    // Start is called before the first frame update
    void Start()
    {
        InitializeQIntArrays();

        //lines below for debugging purposes
        Debug.Log($"Dictionary count: {qClickedCount.Count}");
        Debug.Log($"questionCount from questionSetup script is {questionSetup.questionsCount}");
        
        //debuggingStuff(); //please uncomment the function first before using this
    }

    public void setAvgCalcAnsPPref()
    {
        float x = calcAvgAnsAcc();
        setAvgAnsAcc.Invoke(x.ToString());
        //Debug.Log($"The calculated average answer accuracy is {x}");

    }

    /* =================== debuggingStuff() METHOD ==================== (remove this method from the comment block to use)

    void debuggingStuff() //method that holds some functions to debug (stored in a method for tidying up code purposes)
    {
        //For each loop for the purposes of debugging
        //(Display all of the key value pairs in the dicitonaries)

        foreach (var kvp in qClickedCount) //shows all array's values from qClickedCount dict
        {
            int arrayNameTest = kvp.Key;
            int[] arrayTest = kvp.Value;

            Debug.Log($"Array qClickedCount '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }

        foreach (var kvp in indvAnsAcc) //shows all array's values from indvAnsAcc dict
        {
            int arrayNameTest = kvp.Key;
            float[] arrayTest = kvp.Value;

            Debug.Log($"Array indvAnsAcc '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }
        

        Debug.Log($"The count of questions is {questionSetup.questionsCount}"); //shows the question count
    }
    */

    public void setCurrentIntArr() //sets currentQuestion array to the array from qClickedCount that's corresponding with the current question
    {
       currentQuestion = GetIndvArray(questionSetup.QuestionIndex, qClickedCount);
    }

    public void addCount() //method to add the clicked count in the current question's array (this method is currently only called in QuestionSetup script)
    {
        currentQuestion[0] += 1;
    }
    

    public void InitializeQIntArrays() //create int arrays to hold clicked counts and individual answer accuracies
    {
        for (int i = 0; i < questionSetup.questionsCount; i++) 
        {
            int arrayNum = i;
            int[] newArrayI = new int[1]; 
            float[] newArrayF = new float[1]; 

            // Set all values to 0
            for (int j = 0; j < newArrayI.Length; j++)
            {
                newArrayI[j] = 0;
                newArrayF[j] = 0;
            }

            // Add the array to the dictionaries
            AddToQClicked(arrayNum, newArrayI);
            AddToAnsQuest(arrayNum, newArrayF);
            //Debug.Log($"dict set {i}"); //debug indicator to show each dictionary set
            
        }
    }

    private void AddToQClicked(int arrayName, int[] array) //individual method to add the array into the question clicked count dictionary (will only be called in InitializeQIntArrays)
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
    private void AddToAnsQuest(int arrayNum, float[] array) //individual method to add the array into the individual answer accuracy dictionary (will only be called in InitializeQIntArrays)
    {
        if (!indvAnsAcc.ContainsKey(arrayNum))
        {
            indvAnsAcc.Add(arrayNum, array);
        }
        else
        {
            Debug.LogWarning($"Array '{arrayNum}' already exists in the dictionary.");
        }

    }


    //====================================================


    private T[] GetIndvArray<T>(int arrayValue, Dictionary<int, T[]> dictionary) //method to get a specific array from any dictionary put into the parameter
    {
        if (dictionary.TryGetValue(arrayValue, out T[] array))
        {
            //Debug.Log($"Array {arrayValue} found in dictionary");
            return array;
        }
        else
        {
            //Debug.LogError($"Array '{arrayValue}' not found in the dictionary.");
            return null;
        }
    }
    
    
    public void storeIndvAns() //method to store the calculated answer accuracy of a question into indvAnsAcc dictionary
    {
        float calculatedNum = calcIndvAnsAc(questionSetup.QuestionIndex);  //first, calculate question answer accuracy based on the current question index
        float[] currentArray = GetIndvArray(questionSetup.QuestionIndex, indvAnsAcc); //get the individual array from indvAnsAcc dict based on the current question.

        currentArray[0] = calculatedNum; //store the calculated answer accuracy into the corresponding array of the current question

        // for each loop for debugging purposes (displays each array in the dict to show the changes made)
        foreach (var kvp in indvAnsAcc)
        {
            int arrayNameTest = kvp.Key;
            float[] arrayTest = kvp.Value;

            Debug.Log($"Array '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }
    }

    private float calcIndvAnsAc(int array) //method that calculates the answer accuracy of a question
    {
        int[] currentArray = GetIndvArray(array, qClickedCount); //find array matching from qClickedCount to the current question (or int of the current question)
        Debug.Log($"This question array has {currentArray[0]} clicks");
        float totalClicked = currentArray[0];

        float calcIndvAns = 1 / totalClicked;

        /*
        Convert 25% to 0% WITHOUT using an if-conditional
        */
        float cutoff = 1-(int) (calcIndvAns+0.7f); // if 25% return 1, else return 0
        float newAccu = calcIndvAns - (0.25f*cutoff);
        
        Debug.Log($"calculated answer accuracy for question {array + 1}: {newAccu} (before, it was {calcIndvAns})");
        return newAccu;
    }


    public float calcAvgAnsAcc() //calculate the average answer accuracy
    {
        float sumAnsAcc = 0;

        for (int i = 0; i < questionSetup.questionsCount; i++) //iterate through question array to add to sum
        {
            float[] questionArr = GetIndvArray(i, indvAnsAcc);
            float currentAnsCalc = questionArr[0];
            
            sumAnsAcc += currentAnsCalc;
        }
        
        float rawAnsAcc = sumAnsAcc / questionSetup.questionsCount;  //calculation here
        float totalAnsAcc = rawAnsAcc * 100; //change to percentage form
        totalAnsAcc = Mathf.RoundToInt(totalAnsAcc * 100f) / 100f; //round up the number

        return totalAnsAcc;
    }

    

}
