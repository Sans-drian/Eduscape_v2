using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public bool isHTPMenuOn; //is how to play menu on
    public bool isConfirmExitOn; //is confirm exit menu on
    [SerializeField]
    private GameObject pauseMenuObj;
    [SerializeField]
    private GameObject mainPauseMenuObj; 
    [SerializeField]
    private GameObject UIObject;
    [SerializeField]
    private GameObject howToPlayMenuObj;
    [SerializeField]
    private GameObject confirmExitMenuObj; 


    [SerializeField]
    private PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //check for player pressing escape key
        {
            openPauseMenu();
        }
    }

    public void openPauseMenu() //simple open pause menu method
    {
        if (!isHTPMenuOn && !isConfirmExitOn) //if how to play menu and confirm to exit menu isnt on 
        {
            if(!isPaused) //if it's not paused
            {
                isPaused = true;
                UIObject.SetActive(false);
                pauseMenuObj.SetActive(true);

                playerMovement.GetComponent<PlayerMovement>().canMove = false;            
            }
            else if(isPaused)//if it is paused
            {
                isPaused = false;
                UIObject.SetActive(true);
                pauseMenuObj.SetActive(false);
                
                playerMovement.GetComponent<PlayerMovement>().canMove = true;
            }
        }
        else if (isHTPMenuOn && !isConfirmExitOn)//if the how to play menu is on and confirm to exit menu is off
        {
            howToPlayMenuToggle();
        }
        else if (isConfirmExitOn && !isHTPMenuOn) // if the confirm to exit menu is on and how to play menu is off
        {
            confirmExitMenuToggle();
        }

    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void howToPlayMenuToggle()
    {
        if(!isHTPMenuOn)
        {
            isHTPMenuOn = true;
            mainPauseMenuObj.SetActive(false);
            howToPlayMenuObj.SetActive(true);
        }
        else
        {
            isHTPMenuOn = false;
            howToPlayMenuObj.SetActive(false);
            mainPauseMenuObj.SetActive(true);
        }

    }

    public void confirmExitMenuToggle()
    {
        if(!isConfirmExitOn)
        {
            isConfirmExitOn = true;
            mainPauseMenuObj.SetActive(false);
            confirmExitMenuObj.SetActive(true);
        }
        else
        {
            isConfirmExitOn = false;
            confirmExitMenuObj.SetActive(false);
            mainPauseMenuObj.SetActive(true);
        }

    }
}
