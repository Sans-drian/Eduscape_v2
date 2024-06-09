using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject UIObject;
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
        if(!isPaused)
        {
            isPaused = true;
            UIObject.SetActive(false);
            pauseMenu.SetActive(true);

            playerMovement.GetComponent<PlayerMovement>().canMove = false;            
        }
        else if(isPaused)
        {
            isPaused = false;
            UIObject.SetActive(true);
            pauseMenu.SetActive(false);
            
            playerMovement.GetComponent<PlayerMovement>().canMove = true;
        }
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
