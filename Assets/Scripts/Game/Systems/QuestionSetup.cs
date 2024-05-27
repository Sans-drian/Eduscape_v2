using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Il2Cpp;
using System.Runtime.InteropServices.WindowsRuntime;

public class QuestionSetup : MonoBehaviour
{
    [SerializeField]
    private List<QuestionData> questions; //list that holds all questions
    private QuestionData currentQuestion; //keeps track of which question we're on

    [SerializeField]
    private TextMeshProUGUI questionText; 
    [SerializeField]
    private TextMeshProUGUI categoryText;
    [SerializeField]
    public AnswerButton[] answerButtons;

    [SerializeField]
    private ChestManager chestList;

    [SerializeField]
    private int correctAnswerChoice; //keeps track which is correct answer

    private void Awake() 
    {
        //Get all questions ready
        GetQuestionAssets();
    }

    // Start is called before the first frame update
    void Start()
    {
        chestList = FindObjectOfType<ChestManager>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runDisplayMethods() //run the function from Question Setup script, in this order.
    {
        SelectNewQuestion(); //Get a new question
        SetQuestionValues(); //Set all text and values on screen
        SetAnswerValues(); //Set all of the answer buttons and correct answer values
    }

    public void GetQuestionAssets()
    {
        //Get all questions from question folder
        questions = new List<QuestionData>(Resources.LoadAll<QuestionData>("Questions"));
    }

    public void SelectNewQuestion()
    {
        //Get a random value for which question to choose
        //int randomQuestionIndex = Random.Range(0, questions.Count);



        int QuestionIndex = chestList.clickedObjectIndex; //grab the question from the index id of the chest

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
            //Create a temporary boolean to pass the buttons
            bool isCorrect = false;

            //If it is the correct answer, set the bool to true
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
            }
            
            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
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
        bool correctAnswerChosen = false;

        List<string> newList = new List<string>();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            //Get a random number of the remaining choises
            int random = Random.Range(0, originalList.Count);

            //If the random number is 0, this is the correct answer, MAKE SURE THIS IS ONLY USED ONCE
            if (random == 0 && !correctAnswerChosen)
            {
                correctAnswerChoice = i;
                correctAnswerChosen = true;
            }

            //Add this to the new list
            newList.Add(originalList[random]);
            //Remove this choice from the original list (it has been used)
            originalList.RemoveAt(random);
        }
        return newList;
    }


    
}
