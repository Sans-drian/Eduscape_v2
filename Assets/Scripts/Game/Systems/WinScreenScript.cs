using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownTxt;

    public GameObject winMenu;
    public GameObject countdownScreen;
    public GameObject infoMenu;

    [SerializeField]
    private TextMeshProUGUI playerNameDisplay;
    [SerializeField]
    private TextMeshProUGUI averageAnsAccDisplay;
    [SerializeField]
    private TextMeshProUGUI elapsedTimeDisplay;
    [SerializeField]
    private TextMeshProUGUI questionListDisplay;

    public UnityEvent saveDataToDatabase;


    private string txtColor = "#B0FF58"; // texts will have this color

    void Start()
    {
        setDisplayText();
        saveDataToDatabase.Invoke();
    }

    public void toMainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void callPlayAgain()
    {
        StartCoroutine(playAgain());
    }

    private IEnumerator playAgain()
    {
        countdownScreen.SetActive(true);
        winMenu.SetActive(false);
        /*
        TIMER FUNCTION (5 seconds):
        This implementation of the timer countdown is very bad, but works for now, please improve upon it in the future, to whom ever sees this.
        */
        countDownTxt.text = $"<color={txtColor}>Replaying game in 5...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Replaying game in 4...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Replaying game in 3...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Replaying game in 2...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Replaying game in 1...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Replaying game in 0...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Starting game!</color>";
        yield return new WaitForSeconds(1);

        Debug.Log("Changing game scene...");
        SceneManager.LoadScene("Game"); // change scene to game
    }


    public void openInfoMenu()
    {
        infoMenu.SetActive(true);
        winMenu.SetActive(false);
    }

    public void closeInfoMenu()
    {
        winMenu.SetActive(true);
        infoMenu.SetActive(false);
    }



    private void setDisplayText()
    {
        //get the strings of the playerprefs and set them into a temporary string variable
        string playerName = PlayerPrefs.GetString("PlayerName");
        string avgAnsAcc = PlayerPrefs.GetString("AvgCalcAns");
        string elapsedTime = PlayerPrefs.GetString("ElapsedTime");
        string questionList = PlayerPrefs.GetString("QListName");

        //change the texts on the screen
        playerNameDisplay.text = $"{playerName}'s Results:";
        averageAnsAccDisplay.text = $"{avgAnsAcc}%";
        elapsedTimeDisplay.text = $"{elapsedTime}";
        questionListDisplay.text = $"{questionList}";
    }
}
