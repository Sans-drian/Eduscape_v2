using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private bool isPaused;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openPauseMenu();
        }
    }

    public void openPauseMenu()
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
}
