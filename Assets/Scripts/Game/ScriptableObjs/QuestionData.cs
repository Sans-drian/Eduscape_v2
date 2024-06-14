using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// QuestionData Scriptable object which holds the necessary data for each question
public class QuestionData : ScriptableObject
{
    public string question;
    public string category;
    [Tooltip("The correct answer should always be listed first, they are randomized later")] 
    public string[] answers;
}
