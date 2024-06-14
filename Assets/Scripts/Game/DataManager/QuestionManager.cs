using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    //this acts as a static class/script which can hold the questionList List of QuestionData Scriptable Objects and persist its data throughout the game's runtime.
    public static QuestionManager Instance { get; private set;}

    public List<QuestionData> questionList = new List<QuestionData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction on scene change
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

}
