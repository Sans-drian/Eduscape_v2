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
            pauseMenu.SetActive(true);

            playerMovement.GetComponent<PlayerMovement>().canMove = false;            
        }
        else if(isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            
            playerMovement.GetComponent<PlayerMovement>().canMove = true;
        }
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
