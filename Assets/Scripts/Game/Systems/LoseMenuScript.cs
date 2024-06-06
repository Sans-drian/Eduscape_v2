using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseMenuScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownTxt;

    public GameObject loseMenu;
    public GameObject countdownScreen;
    private string txtColor = "#B0FF58"; //non-error texts will have this color

    
    public void mainMenu() //run method when player presses the Main Menu button
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void tryAgain() //run method when player presses Try Again button
    {
        StartCoroutine (loadGame());
    }

    private IEnumerator loadGame()
    {
        countdownScreen.SetActive(true);
        loseMenu.SetActive(true);
        /*
        TIMER FUNCTION (5 seconds):
        This implementation of the timer countdown is very bad, but works for now, please improve upon it in the future, to whom ever sees this.
        */
        countDownTxt.text = $"<color={txtColor}>Retrying game in 5...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Retrying game in 4...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Retrying game in 3...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Retrying game in 2...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Retrying game in 1...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Retrying game in 0...</color>";
        yield return new WaitForSeconds(1);
        countDownTxt.text = $"<color={txtColor}>Starting game!</color>";
        yield return new WaitForSeconds(1);

        Debug.Log("Changing game scene...");
        SceneManager.LoadScene("Game"); // change scene to game
    }
}
