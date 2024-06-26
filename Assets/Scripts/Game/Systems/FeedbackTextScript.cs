using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackTextScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI feedbackTxt;
    [SerializeField]
    private GameObject feedbackTxtObj;

    [SerializeField]
    private TextMeshProUGUI isAnsweredFeedbackText;
    [SerializeField]
    private GameObject isAnsweredFeedbackObj;

    [SerializeField]
    private Timer timer;
    private float timeDecreaseNum;
    private Coroutine displayCoroutine;
    private Coroutine isAnsweredCoroutine;

    private string penaltyTime;

    private string wrongTxtColor = "#FF6666"; //wrong answer text will have this color
    private string correctTxtColor = "#B0FF58"; //correct answer text will have this color
    private string blueTxtColor = "#40C3BF"; //custom blue text color

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        timeDecreaseNum = timer.getTimeDecrease();

        setPenaltyText();
        //Debug.Log($"Penalty time: {penaltyTime}");
    }

    public void callDisplayCorrectTxt()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        displayCoroutine = StartCoroutine(displayCorrectTxt(4f));
    }

    private IEnumerator displayCorrectTxt(float duration)
    {
        //Debug.Log("Displaying correct feedback");
        feedbackTxtObj.SetActive(false);
        yield return new WaitForSeconds(.1f);
        feedbackTxtObj.SetActive(true);
        feedbackTxt.text = $"<color={correctTxtColor}>Correct! +1 Key</color>";
        yield return new WaitForSeconds(duration);
        feedbackTxtObj.SetActive(false);
    }

    public void callDisplayWrongTxt()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        displayCoroutine = StartCoroutine(displayWrongTxt(3f));
    }

    private IEnumerator displayWrongTxt(float duration)
    {
        //Debug.Log("Displaying wrong feedback");
        feedbackTxtObj.SetActive(false);
        yield return new WaitForSeconds(.1f);
        feedbackTxtObj.SetActive(true);
        feedbackTxt.text = $"<color={wrongTxtColor}>Wrong! -{penaltyTime}</color>";
        yield return new WaitForSeconds(duration);
        feedbackTxtObj.SetActive(false);
    }

    public void callDisplayIsAnswwered()
    {
        if (isAnsweredCoroutine != null)
        {
            StopCoroutine(isAnsweredCoroutine);
        }

        isAnsweredCoroutine = StartCoroutine(displayIsAnswered(4f));
    }

    private IEnumerator displayIsAnswered(float duration)
    {
        isAnsweredFeedbackObj.SetActive(false);
        yield return new WaitForSeconds(.1f);
        isAnsweredFeedbackObj.SetActive(true);
        isAnsweredFeedbackText.text = $"<color={blueTxtColor}>This chest is already answered!</color>";
        yield return new WaitForSeconds(duration);
        isAnsweredFeedbackObj.SetActive(false);
    }



    private void setPenaltyText()
    {
        bool secondsExist = checkSecondsExists();

        int minutes = Mathf.FloorToInt(timeDecreaseNum / 60);
        int seconds = Mathf.FloorToInt(timeDecreaseNum % 60);

        string timeText;
        if (minutes > 0) // if timeDecreaseNum is greater than 60 seconds
        {
            // display minutes (while also checking to use singular or plural)
            timeText = $"{minutes} minute{(minutes != 1 ? "s" : "")}";
            
            if (secondsExist) //if seconds exist, then append seconds text
            {
                timeText += $" and {seconds} second{(seconds != 1 ? "s" : "")}";
            }
        }
        else // if timeDecreaseNum is less than 60 seconds
        {
            // display seconds (while also checking to use singular or plural)
            timeText = $"{seconds} second{(seconds != 1 ? "s" : "")}";
        }
            
        penaltyTime = $"{timeText}";
    }

    private bool checkSecondsExists() //method to check whether seconds exist or not in the time inputted (used for formatting the text, to either use or not use "second(s)")
    {
        int seconds = Mathf.FloorToInt(timeDecreaseNum % 60);
        return seconds != 0;
    }



}
