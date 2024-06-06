using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.InputSystem;
using System.Threading;

public class QuestionSetup : MonoBehaviour
{
    [SerializeField]
    private List<QuestionData> questions; //list that holds all questions
    public int questionsCount; //variable to be used for CalculateAnsAcc script

    private QuestionData currentQuestion; //keeps track of which question we're on
    public int QuestionIndex;
    public int buttonID;

    public CalculateAnsAcc calculateAnsAcc;

    [SerializeField]
    private TextMeshProUGUI questionText; 
    [SerializeField]
    private TextMeshProUGUI categoryText;
    [SerializeField]
    public AnswerButton[] answerButtons;

    private Dictionary<int, bool[]> buttonStates = new Dictionary<int, bool[]>(); //dictionary to hold button states array
    List<int> randList = new List<int>();
    bool[] retrievedArray;

    [SerializeField]
    private ChestManager chestList;

    [SerializeField]
    private int correctAnswerChoice; //keeps track which is correct answer

    private void Awake() 
    {
        chestList = FindObjectOfType<ChestManager>(); 

        //calculateAnsAcc = FindObjectOfType<CalculateAnsAcc>();
        
        if (calculateAnsAcc != null)
        {
            Debug.Log("calculateAnsAcc is not null");
        }
        else
        {
            Debug.LogError("calculateAnsAcc is null");
        }


        //Get all questions ready
        GetQuestionAssets();
        InitializeButtonBoolArrays();
        //Debug.Log($"original question list count from QuestionSetup script is {questions.Count}");
        questionsCount = questions.Count;
        //Debug.Log($"questionsCount int set from original question list from QuestionSetup script is {questionsCount}");

        foreach (var kvp in buttonStates) //prints out all of the key value pairs of buttonStates
        {
            int arrayNameTest = kvp.Key;
            bool[] arrayTest = kvp.Value;

            //Debug.Log($"Array '{arrayNameTest}': {string.Join(", ", arrayTest)}");
        }
    }


    public void runDisplayMethods() //run the function from Question Setup script, in this order.
    {
        SelectNewQuestion(); //Get a new question
        SetQuestionValues(); //Set all text and values on screen
        SetAnswerValues(); //Set all of the answer buttons and correct answer values
    }

    public void InitializeButtonBoolArrays() //create boolean arrays to hold button states
    {
        for (int i = 0; i < questions.Count; i++)
        {
            int arrayName = i;
            bool[] newArray = new bool[4]; // Change the size as needed

            // Set all values to false
            for (int j = 0; j < newArray.Length; j++)
            {
                newArray[j] = false;
            }

            // Add the array to the dictionary
            AddBooleanArray(arrayName, newArray);
            
        }
    }

    private void AddBooleanArray(int arrayName, bool[] array)
    {
        if (!buttonStates.ContainsKey(arrayName))
        {
            buttonStates.Add(arrayName, array);
        }
        else
        {
            Debug.LogWarning($"Array '{arrayName}' already exists in the dictionary.");
        }

    }

    private bool[] GetBooleanArray(int arrayValue)
    {
        if (buttonStates.TryGetValue(arrayValue, out bool[] array))
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

    
    private bool GetArrayIndex(int arrayIndex) //grabs the bool index of the thrown in to the parameter array
    {
        bool arrayBool = false; 

        if (arrayIndex >= 0 && arrayIndex < retrievedArray.Length) //checks if arrayIndex is within range of retrievedArray length
        {
            bool valueAtIndex = retrievedArray[arrayIndex];
            arrayBool = valueAtIndex;
            Debug.Log($"Array[{arrayIndex}] value: {valueAtIndex}");
        }
        else
        {
            Debug.LogError($"Invalid index: {arrayIndex}. Array size is {retrievedArray.Length}.");
        }

        Debug.Log($"{arrayBool}");
        return arrayBool;
    }
    
    public void updateBoolAddCount() //update the boolean of the boolean array according to the current question to true
    {
        
        if (!retrievedArray[buttonID]) //if the button boolean is not false
        {
            calculateAnsAcc.addCount();
            retrievedArray[buttonID] = true;
            Debug.Log("Adding count and setting bool of button to true.");
        }
        else 
        {
            Debug.LogWarning("Bool of button is already true, will not be adding count.");
        }
        
    }


    public void GetQuestionAssets()
    {
        //Get all questions from question folder
        questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions"));

        //Debug.Log($"question assets list before shuffle: {string.Join(", ", questions)}"); //Debugging: show list before shuffle
        ShuffleList(questions);
        //Debug.Log($"question assets list after shuffle: {string.Join(", ", questions)}"); //Debugging: show list after shuffle
    }

    private void ShuffleList(List<QuestionData> inputList) //randomize the elements in the list, in turn, randomizing the position of questions
    {
        for (int i = 0; i < questions.Count - 1; i++)
        {
            QuestionData temp = inputList[i];
            int rand = Random.Range(i, questions.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }        
    }


    public void SelectNewQuestion()
    {
        //Get a random value for which question to choose
        //int randomQuestionIndex = Random.Range(0, questions.Count);



        QuestionIndex = chestList.clickedObjectIndex; //grab the question from the index id of the chest
        retrievedArray = GetBooleanArray(QuestionIndex); //setting the boolean array corresponding to the currently clicked question

        if (QuestionIndex >= 0 && QuestionIndex < questions.Count) //checking whether index exists or not
        {
            Debug.Log("This index exists!");

            //Set the question to the question index
            currentQuestion = questions[QuestionIndex];

        }
        else
        {
            Debug.LogError("This index does not exist!");
        }
    }

    public void SetQuestionValues()
    {
        //Set the question text
        questionText.text = currentQuestion.question;
        //Set the category text
        categoryText.text = currentQuestion.category;
    }

    public void SetAnswerValues()
    {
        //Randomize the answer button order
        List<string> answers = RandomizeAnswers(new List<string>(currentQuestion.answers));
        
        
        //Set up the answer buttons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int arrayIndex = randList[i];
            bool valueOfIndex = GetArrayIndex(arrayIndex);
            //Create a temporary boolean to pass the buttons
            bool isCorrect = false;

            //If it is the correct answer, set the bool to true
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
            }
            
            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
            answerButtons[i].SetButtonState(valueOfIndex);
            answerButtons[i].SetButtonID(arrayIndex);
            //Debug.Log($"Value of {i}");
        }

        
    /*
        for (int i = 0; i < answerButtons.Length; i++) //this checks the index/gameobjects of the list of question
        {
            obj = answerButtons[i];
            Debug.Log($"Object at index {i}: {obj}");
        }   
    */
    }

    private List<string> RandomizeAnswers(List<string> originalList)
    {
        randList.Clear();
        List<string> newList = new List<string>();

        //THE WHOLE SETUP IS TO ENSURE UNIQUE RANDOM NUMBERS
        List<int> unshuffled = new List<int>() {0,1,2,3};

        for (int i = 0; i < answerButtons.Length; i++) 
        {
            int random = Random.Range(0, unshuffled.Count);
            randList.Add(unshuffled[random]);
            unshuffled.RemoveAt(random);
        }

        /*
        foreach( var x in originalList) 
        {
            Debug.Log($"FOR EACH LOOP PRINTING: {x}" );
        }
        */

        for (int i = 0; i < answerButtons.Length; i++)
        {

            int random;

            //If the random number is 0, this is the correct answer, MAKE SURE THIS IS ONLY USED ONCE
            if (randList[i] == 0)
            {
                correctAnswerChoice = i;
                random = randList[i];
                //Debug.Log($"if route: {randList[i]}");
            } 
            else 
            {
                random = randList[i];
                //Debug.Log($"else route: {randList[i]}");
            }

            //Add this to the new list
            newList.Add(originalList[random]);

            //Remove this choice from the original list (it has been used)
            //originalList.RemoveAt(random);
        }
        
        return newList;
    }
    
}
