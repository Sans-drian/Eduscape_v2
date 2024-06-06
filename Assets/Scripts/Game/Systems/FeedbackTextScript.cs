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
    private bool isCoroutineRunning = false;

    private string wrongTxtColor = "#FF6666"; //wrong answer text will have this color
    private string correctTxtColor = "#B0FF58"; //correct answer text will have this color

    /*

    NOTE:
    The methods below, callDisplayCorrectTxt and callDisplayWrongTxt and the extra methods called inside work but are buggy if the player calls the method
    multiple times, a.k.a., if the player presses the answer button multiple times while the coroutine is still running (or if the feedback text is still
    set to active).

    I will leave it up to future developers of Eduscape to fix and implement this function properly. Thank you.

    */
    public void callDisplayCorrectTxt()
    {
        if (!isCoroutineRunning) 
        {
            StartCoroutine(displayCorrectTxt());
            isCoroutineRunning = true;
        }
        else
        {
            StopCoroutine(displayCorrectTxt());
            StartCoroutine(displayCorrectTxt());
            isCoroutineRunning = true;
        }
    }

    private IEnumerator displayCorrectTxt()
    {
        feedbackTxtObj.SetActive(true);
        feedbackTxt.text = $"<color={correctTxtColor}>Correct! +1 Key</color>";
        yield return new WaitForSeconds(4);
        feedbackTxtObj.SetActive(false);
        isCoroutineRunning = false;
    }

    public void callDisplayWrongTxt()
    {
        if (!isCoroutineRunning) 
        {
            StartCoroutine(displayWrongTxt());
            isCoroutineRunning = true;
        }
        else
        {
            StopCoroutine(displayWrongTxt());
            StartCoroutine(displayWrongTxt());
            isCoroutineRunning = true;
        }
    }

    private IEnumerator displayWrongTxt()
    {
        feedbackTxtObj.SetActive(true);
        feedbackTxt.text = $"<color={wrongTxtColor}>Wrong! -10 seconds</color>";
        yield return new WaitForSeconds(3);
        feedbackTxtObj.SetActive(false);
        isCoroutineRunning = false;
    }

}
