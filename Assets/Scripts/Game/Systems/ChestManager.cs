using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestManager : MonoBehaviour
{
    public List<GameObject> chests = new List<GameObject>(); // List to hold chest game objects
    public int clickedObjectIndex;
  

    
    public QuestionSetup questionSetup;
    public GameObject quizMenu;
    public bool isInteractingChest;
    public PlayerMovement playerMovement;
    public PauseMenu pauseMenu;

    public UnityEvent isAnsweredMsg;

    void Awake()
    {
        questionSetup = FindObjectOfType<QuestionSetup>(); //this might be useless.. (doesnt do anything? but keep it regardless)
        pauseMenu = GetComponent<PauseMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < chests.Count; i++) //this checks the index/gameobjects of the list of question
        {
            GameObject obj = chests[i];
            Debug.Log($"Object at index {i}: {obj.name}");
        }    
        */ 
    }

    public void checkWhichChest(GameObject obj) //this function sets the value of which chest the player is interacting with
    {
        clickedObjectIndex = chests.IndexOf(obj);

        /* DEBUGGING CODE BELOW
        if (clickedObjectIndex != -1)
        {
            Debug.Log($"Clicked object (parent) is at index {clickedObjectIndex}");
        }
        else
        {
            Debug.LogError("Object not found in the list.");
        }
        */
    }


    public void openQuizMenu()
    {
        GameObject interactingChest = chests[clickedObjectIndex]; // Get the specific GameObject
        var chest = interactingChest.GetComponent<Chest>(); // place the component into the variable chest

        if (!pauseMenu.isPaused)   
        {    
            if (!chest.isAnswered) //if the chest is not answered
            {
                //Debug.Log("Chest is not answered");

                if (!isInteractingChest) 
                {
                    isInteractingChest = true;
                    quizMenu.SetActive(true);
                    playerMovement.GetComponent<PlayerMovement>().canMove = false;
                    
                    displayQuestion();
                    
                }
                else if (isInteractingChest)
                {
                    quizMenu.SetActive(false);
                    isInteractingChest = false;
                    
                    playerMovement.GetComponent<PlayerMovement>().canMove = true;
                }
            }
            else
            {
                //Debug.Log("Chest is already answered");
                isAnsweredMsg.Invoke();
            }
        }
        else
        {
            Debug.Log("Currently paused, not able to interact.");
        }
    }

    public void displayQuestion()
    {

        if (questionSetup != null) //debugging purposes
        {
            questionSetup.SelectNewQuestion();
            questionSetup.SetQuestionValues();
            questionSetup.SetAnswerValues();
        }
        else
        {
            Debug.LogError("QuestionSetup is null. Cannot display question.");
        }
    }

    public void disableChest()
    {
        GameObject interactingChest = chests[clickedObjectIndex]; // Get the specific GameObject
        var chest = interactingChest.GetComponent<Chest>(); // place the component into the variable chest

        chest.isAnswered = true;
        chest.animator.SetBool("IsAnswered", true);
        playerMovement.GetComponent<PlayerMovement>().canMove = true;
        quizMenu.SetActive(false);
        isInteractingChest = false;
    }
}
